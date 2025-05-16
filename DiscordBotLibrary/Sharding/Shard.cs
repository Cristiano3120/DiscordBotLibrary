using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DiscordBotLibrary.Sharding
{
    internal sealed class Shard(int shardId)
    {
        internal ResumeConnInfos ResumeConnInfos { get; set; } = ResumeConnInfos.EmptyConnInfos;
        private ClientWebSocket _webSocket = new();
        private int? _lastSequenceNumber = null;
        private readonly int _shardId = shardId;

        public async Task StartShardAsync()
        {
            Uri gatewayUri = new($"wss://gateway.discord.gg/?v={DiscordClient.ClientConfig.Version}&encoding=json");
            await _webSocket.ConnectAsync(gatewayUri, CancellationToken.None);

            _ = ReceiveMessagesAsync();
        }

        internal async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[16384];
            MemoryStream ms = new();

            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    WebSocketReceiveResult receivedDataInfo = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (receivedDataInfo.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        break;
                    }

                    await ms.WriteAsync(buffer.AsMemory(0, receivedDataInfo.Count));
                    if (!receivedDataInfo.EndOfMessage)
                    {
                        continue;
                    }

                    string message = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                    //Logger.LogPayload(ConsoleColor.Cyan, message, "[RECEIVED]:");

                    JsonDocument jsonDocument = JsonDocument.Parse(message);
                    await HandleReceivedMessage(jsonDocument);

                    ClearMs(ms);
                }
                catch (Exception ex)
                {
                    DiscordClient.Logger.LogError(ex);
                }
            }

            DiscordClient.Logger.LogDebug("Connection closed!");
        }

        private async Task HandleReceivedMessage(JsonDocument jsonDocument)
        {
            JsonElement message = jsonDocument.RootElement;
            OpCode opCode = message.GetOpCode();

            try
            {
                switch (opCode)
                {
                    case OpCode.Dispatch:
                        _lastSequenceNumber = HandleDiscordPayload.HandleDispatch(this, message);
                        break;
                    case OpCode.Hello:
                        DiscordClient.Logger.LogInfo("Received Hello message.");
                        await HandleDiscordPayload.HandleHelloEvent(message, this, ResumeConnInfos, _lastSequenceNumber);
                        break;
                    case OpCode.HeartbeatAck:
                        DiscordClient.Logger.LogDebug("Heartbeat acknowledged.");
                        break;
                    case OpCode.Reconnect:
                        DiscordClient.Logger.LogWarning("Reconnect requested.");
                        await HandleDiscordPayload.HandleResumeConnAsync(_webSocket, ResumeConnInfos);
                        break;
                    case OpCode.InvalidSession:
                        DiscordClient.Logger.LogWarning("Invalid session.");
                        await HandleDiscordPayload.HandleSessionInvalidAsync(this, message, ResumeConnInfos, _webSocket);
                        break;
                }
            }
            catch (Exception ex)
            {
                DiscordClient.Logger.LogError(ex);
            }
        }

        internal void InvokeEvent(Events events, JsonElement jsonElement)
        {
            try
            {
                switch (events)
                {
                    case Events.READY:
                        HandleDiscordPayload.HandleReadyEvent(this, jsonElement);
                        break;
                    case Events.GUILD_CREATE:
                        HandleDiscordPayload.HandleGuildCreateEvent(jsonElement, _shardId);
                        break;
                    default:
                        DiscordClient.Logger.LogWarning($"Unhandled event: {events}.");
                        break;
                }
            }
            catch (Exception ex)
            {
                DiscordClient.Logger.LogError(ex);
            }
        }

        public async Task SendHeartbeatsAsync(int interval)
        {
            while (_webSocket.State == WebSocketState.Open)
            {
                DiscordClient.Logger.LogDebug($"Sending heartbeat. Sequence: {_lastSequenceNumber}");
                var heartbeatPayload = new
                {
                    op = OpCode.Heartbeat,
                    d = _lastSequenceNumber
                };

                await SendPayloadWssAsync(heartbeatPayload);
                await Task.Delay(interval);
            }
        }

        internal async Task SendPayloadWssAsync(object payload)
        {
            string jsonStr = JsonSerializer.Serialize(payload, DiscordClient.JsonSerializerOptions);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonStr);

            Logger.LogPayload(ConsoleColor.Cyan, jsonStr, "[SENT]:");
            await _webSocket.SendAsync(jsonBytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        internal async Task SendPayloadWssAsync(string jsonStr)
        {
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonStr);

            Logger.LogPayload(ConsoleColor.Cyan, jsonStr, "[SENT]:");
            await _webSocket.SendAsync(jsonBytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        internal async Task SendIdentifyAsync()
        {
            DiscordClient.Logger.LogInfo("Sending Identify payload");

            const string clientId = "DiscordBotLibrary";
            var identifyPayload = new
            {
                op = OpCode.Identify,
                d = new
                {
                    token = DiscordClient.ClientConfig.Token,
                    properties = new
                    {
                        os = Environment.OSVersion.Platform.ToString(),
                        browser = clientId,
                        device = clientId,
                    },
                    intents = DiscordClient.ClientConfig.Intents,
                    shard = new[]
                    {
                        _shardId,
                        ShardHandler.TotalShards
                    }
                },
            };

            await SendPayloadWssAsync(identifyPayload);
        }

        internal async Task RestartShardAsync()
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
            }

            _webSocket = new ClientWebSocket();
            ResumeConnInfos = ResumeConnInfos.EmptyConnInfos;
            _lastSequenceNumber = null;
            //Guilds = [];

            await StartShardAsync();
        }

        private static void ClearMs(MemoryStream ms)
        {
            byte[] buffer = ms.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            ms.Position = 0;
            ms.SetLength(0);
        }
    }
}
