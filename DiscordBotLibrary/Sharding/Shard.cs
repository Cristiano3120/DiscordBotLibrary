using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DiscordBotLibrary.Sharding
{
    internal sealed class Shard(int shardId)
    {
        internal ResumeConnInfos ResumeConnInfos { get; set; } = ResumeConnInfos.EmptyConnInfos;
        private readonly CancellationTokenSource _cts = new();
        private WsGatewayLimiter _wsGatewayLimiter = default!;
        private ClientWebSocket _webSocket = new();
        private readonly int _shardId = shardId;
        private int? _lastSequenceNumber;

        public Dictionary<string, (TaskCompletionSource<List<GuildMember>>, List<GuildMember>)> GuildMemberRequests { get; init; } = new();

        internal async Task StartShardAsync()
        {
            Uri gatewayUri = new($"wss://gateway.discord.gg/?v={DiscordClient.ClientConfig.Version}&encoding=json");
            await _webSocket.ConnectAsync(gatewayUri, _cts.Token);

            _wsGatewayLimiter = new WsGatewayLimiter(this);
            _ = ReceiveMessagesAsync();
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

            await StartShardAsync();
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
                    DiscordClient.Logger.LogPayload(ConsoleColor.Cyan, message, PayloadType.Received);

                    JsonDocument jsonDocument = JsonDocument.Parse(message);
                    await HandleReceivedMessage(jsonDocument);

                    ClearMs(ms);
                }
                catch (Exception ex)
                {
                    DiscordClient.Logger.LogError(ex);
                    _cts.Cancel();
                }
            }

            DiscordClient.Logger.LogDebug("Connection closed!");
            DiscordClient.Logger.LogDebug($"CloseCode: {_webSocket.CloseStatus}, Reason: {_webSocket.CloseStatusDescription}");
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
                        await HandleDiscordPayload.HandleHelloEventAsync(message, this, ResumeConnInfos, _lastSequenceNumber);
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

        internal void InvokeEvent(Event events, JsonElement jsonElement)
        {
            try
            {
                switch (events)
                {
                    case Event.READY:
                        HandleDiscordPayload.HandleReadyEvent(this, jsonElement);
                        break;
                    case Event.GUILD_CREATE:
                        HandleDiscordPayload.HandleGuildCreateEvent(jsonElement);
                        break;
                    case Event.PRESENCE_UPDATE:
                        HandleDiscordPayload.HandlePresenceUpdateEvent(jsonElement);
                        break;
                    case Event.VOICE_SERVER_UPDATE:
                        HandleDiscordPayload.HandleVoiceServerUpdateEvent(jsonElement);
                        break;
                    case Event.GUILD_MEMBERS_CHUNK:
                        HandleDiscordPayload.HandleGuildMembersChunkEvent(jsonElement, this);
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

        #region Send

        internal async Task SendPayloadWssAsync(object payload, bool isHeartbeat = false)
        {
            string jsonStr = JsonSerializer.Serialize(payload, DiscordClient.JsonSerializerOptions);

            if (isHeartbeat)
            {
                await SendPayloadWssAsync(jsonStr);
                return;
            }

            if (!await _wsGatewayLimiter.TrySendAsync(_webSocket, jsonStr))
            {
                DiscordClient.Logger.LogInfo($"Shard{_shardId}: Gateway limit reached!");
            }
        }

        internal async Task SendPayloadWssAsync(string jsonStr)
        {
            try
            {
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonStr);

                DiscordClient.Logger.LogPayload(ConsoleColor.Cyan, jsonStr, PayloadType.Sent);
                await _webSocket.SendAsync(jsonBytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                DiscordClient.Logger.LogError(ex);
            }
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
                    intents = DiscordClient.ClientConfig.Intents,
                    token = DiscordClient.ClientConfig.Token,
                    properties = new
                    {
                        os = Environment.OSVersion.Platform.ToString(),
                        browser = clientId,
                        device = clientId,
                    },
                    shard = new[]
                    {
                        _shardId,
                        ShardHandler.TotalShards
                    }
                },
            };

            await SendPayloadWssAsync(identifyPayload);
        }

        internal async Task SendHeartbeatsAsync(int interval)
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

        internal async Task SendGuildMemberRequestAsync(TaskCompletionSource<List<GuildMember>> tcs, RequestGuildMembers requestGuildMembers)
        {
            GuildMemberRequests.Add(requestGuildMembers.Nonce, (tcs, []));

            var payload = new
            {
                op = OpCode.RequestGuildMembers,
                d = requestGuildMembers
            };

            await SendPayloadWssAsync(payload);
        }

        #endregion

        private static void ClearMs(MemoryStream ms)
        {
            byte[] buffer = ms.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            ms.Position = 0;
            ms.SetLength(0);
        }
    }
}
