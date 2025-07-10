using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using DiscordBotLibrary.WssPayloadStructures.Identify;

namespace DiscordBotLibrary.Sharding
{
    internal sealed class Shard(DiscordClientConfig discordClientConfig, HandleDiscordPayload handleDiscordPayload, int shardId)
    {
        private readonly HandleDiscordPayload _handleDiscordPayload = handleDiscordPayload;
        private readonly DiscordClientConfig _clientConfig = discordClientConfig;

        private readonly CancellationTokenSource _cts = new();
        private WsGatewayLimiter _wsGatewayLimiter = default!;
        private ClientWebSocket _webSocket = new();

        private readonly int _shardId = shardId;
        private int? _lastSequenceNumber;

        public Dictionary<string, RequestGuildMembersCache> GuildMemberRequests { get; init; } = new();
        public Dictionary<ulong, TaskCompletionSource<SoundboardSound[]>> SoundboardRequests { get; init; } = new();
        internal ResumeConnInfos ResumeConnInfos { get; set; } = ResumeConnInfos.EmptyConnInfos;

        internal async Task StartShardAsync()
        {
            Uri gatewayUri = new($"wss://gateway.discord.gg/?v={_clientConfig.Version}&encoding=json");
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
            byte[] rentedBuffer = ArrayPool<byte>.Shared.Rent(16384);
            Memory<byte> buffer = new(rentedBuffer);
            MemoryStream ms = new();

            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    ValueWebSocketReceiveResult receivedDataInfo = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    if (receivedDataInfo.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        break;
                    }

                    await ms.WriteAsync(buffer.Slice(0, receivedDataInfo.Count));
                    if (!receivedDataInfo.EndOfMessage)
                    {
                        continue;
                    }

                    string message = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                    DiscordClient.Logger.LogWssPayload(PayloadType.Received, message, _shardId);

                    JToken jToken = JToken.Parse(message);
                    await HandleReceivedMessage(jToken);

                    ms.Position = 0;
                    ms.SetLength(0);
                }
                catch (Exception ex)
                {
                    DiscordClient.Logger.LogError(ex);
                    _cts.Cancel();
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(rentedBuffer);
                }
            }

            DiscordClient.Logger.Log(LogLevel.Debug, "Connection closed!");
            DiscordClient.Logger.Log(LogLevel.Debug, $"CloseCode: {_webSocket.CloseStatus}, Reason: {_webSocket.CloseStatusDescription}");
        }

        private async Task HandleReceivedMessage(JToken jToken)
        {
            OpCode opCode = jToken.GetOpCode();

            try
            {
                switch (opCode)
                {
                    case OpCode.Dispatch:
                        _lastSequenceNumber = await HandleDiscordPayload.HandleDispatch(this, jToken);
                        break;
                    case OpCode.Hello:
                        DiscordClient.Logger.Log(LogLevel.Info, "Received Hello message.");
                        HelloEventParams helloEventParams = new()
                        {
                            JToken = jToken,
                            Shard = this,
                            ResumeConnInfos = ResumeConnInfos,
                            LastSequenceNumber = _lastSequenceNumber,
                            Token = _clientConfig.Token,
                            ShardCount = DiscordClient.GetDiscordClient().GetTotalShardCount(),
                        };
                        await HandleDiscordPayload.HandleHelloEventAsync(helloEventParams);
                        break;
                    case OpCode.HeartbeatAck:
                        DiscordClient.Logger.Log(LogLevel.Debug, "Heartbeat acknowledged.");
                        break;
                    case OpCode.Reconnect:
                        DiscordClient.Logger.Log(LogLevel.Warning, "Reconnect requested.");
                        await HandleDiscordPayload.HandleResumeConnAsync(_webSocket, ResumeConnInfos);
                        break;
                    case OpCode.InvalidSession:
                        DiscordClient.Logger.Log(LogLevel.Warning, "Invalid session.");
                        await HandleDiscordPayload.HandleSessionInvalidAsync(this, jToken, ResumeConnInfos, _webSocket);
                        break;
                }
            }
            catch (Exception ex)
            {
                DiscordClient.Logger.LogError(ex);
            }
        }

        internal async Task InvokeEvent(Event events, JToken jToken)
        {
            try
            {
                switch (events)
                {
                    case Event.READY:
                        _handleDiscordPayload.HandleReadyEvent(this, jToken);
                        break;
                    case Event.GUILD_CREATE:
                        _handleDiscordPayload.HandleGuildCreateEvent(jToken);
                        break;
                    case Event.PRESENCE_UPDATE:
                        _handleDiscordPayload.HandlePresenceUpdateEvent(jToken);
                        break;
                    case Event.VOICE_SERVER_UPDATE:
                        _handleDiscordPayload.HandleVoiceServerUpdateEvent(jToken);
                        break;
                    case Event.GUILD_MEMBERS_CHUNK:
                        _handleDiscordPayload.HandleGuildMembersChunkEvent(jToken, this);
                        break;
                    case Event.SOUNDBOARD_SOUNDS:
                        HandleDiscordPayload.HandleSoundboardSoundsEvent(jToken, this);
                        break;
                    case Event.VOICE_STATE_UPDATE:
                        _handleDiscordPayload.HandleVoiceStateUpdateEvent(jToken);
                        break;
                    case Event.CHANNEL_CREATE:
                        _handleDiscordPayload.HandleChannelCreateEvent(jToken);
                        break;
                    case Event.CHANNEL_DELETE:
                        _handleDiscordPayload.HandleChannelDeleteEvent(jToken);
                        break;
                    case Event.CHANNEL_UPDATE:
                        _handleDiscordPayload.HandleChannelUpdateEvent(jToken);
                        break;
                    case Event.CHANNEL_PINS_UPDATE:
                        await _handleDiscordPayload.HandleChannelPinsUpdateEvent(jToken);
                        break;
                    default:
                        DiscordClient.Logger.Log(LogLevel.Warning, $"Unhandled event: {events}.");
                        break;
                }
            }
            catch (Exception ex)
            {
                DiscordClient.Logger.LogError(ex);
            }
        }

        #region Send

        internal async Task SendPayloadWssAsync<T>(Payload<T> payload, bool isHeartbeat = false)
        {
            string jsonStr = JsonConvert.SerializeObject(payload, DiscordClient.SendJsonSerializerSettings);

            if (isHeartbeat)
            {
                await SendPayloadWssAsync(jsonStr);
                return;
            }

            if (!await _wsGatewayLimiter.TrySendAsync(_webSocket, jsonStr))
            {
                DiscordClient.Logger.Log(LogLevel.Info, $"Shard{_shardId}: Gateway limit reached!");
            }
        }

        internal async Task SendPayloadWssAsync(string jsonStr)
        {
            try
            {
                DiscordClient.Logger.LogWssPayload(PayloadType.Sent, jsonStr, _shardId);

                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonStr);
                await _webSocket.SendAsync(jsonBytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                DiscordClient.Logger.LogError(ex);
            }
        }

        internal async Task SendIdentifyAsync(int shardCount)
        {
            DiscordClient.Logger.Log(LogLevel.Info, "Sending IdentifyPayload payload");

            const string clientId = "DiscordBotLibrary";
            IdentifyPayload identifyPayload = new()
            {
                Intents = _clientConfig.Intents,
                Token = _clientConfig.Token,
                Shard = [_shardId, shardCount],
                Properties = new Properties()
                {
                    Os = Environment.OSVersion.Platform.ToString(),
                    Browser = clientId,
                    Device = clientId,
                }
            };
            Payload<IdentifyPayload> payload = new(OpCode.Identify, identifyPayload);

            await SendPayloadWssAsync(payload);
        }

        internal async Task SendHeartbeatsAsync(int interval)
        {
            while (_webSocket.State == WebSocketState.Open)
            {
                DiscordClient.Logger.Log(LogLevel.Debug, $"Sending heartbeat. Sequence: {_lastSequenceNumber}");

                Payload<int?> payload = new(OpCode.Heartbeat, _lastSequenceNumber);
                await SendPayloadWssAsync(payload);

                await Task.Delay(interval);
            }
        }

        internal async Task SendGuildMemberRequestAsync(RequestGuildMembersCache requestGuildMembersCache)
        {
            GuildMemberRequests.Add(requestGuildMembersCache.RequestGuildMembers.Nonce, requestGuildMembersCache);

            Payload<RequestGuildMembers> payload = new(OpCode.RequestGuildMembers, requestGuildMembersCache.RequestGuildMembers);
            await SendPayloadWssAsync(payload);
        }

        internal async Task SendSoundboardRequestAsync(RequestSoundboardSounds requestSoundoardSounds)
        {
            foreach ((ulong guildId, TaskCompletionSource<SoundboardSound[]> tcs) in requestSoundoardSounds.TaskCompletionSources)
            {
                SoundboardRequests.Add(guildId, tcs);
            }

            string[] guildIds = [.. requestSoundoardSounds.GuildIds.Select(x => x.ToString())];
            RequestSoundboardSoundsPayload requestSoundboardSoundsPayload = new(guildIds);
            Payload<RequestSoundboardSoundsPayload> payload = new(OpCode.RequestSoundboardSounds, requestSoundboardSoundsPayload);

            await SendPayloadWssAsync(payload);
        }

        #endregion
    }
}
