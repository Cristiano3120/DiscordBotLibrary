using DiscordBotLibrary.Sharding;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;


namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the main class for interacting with the Discord API.
    /// </summary>
    public sealed class DiscordClient
    {
        public List<DiscordGuild> Guilds { get; private set; } = [];
        internal static JsonSerializerOptions JsonSerializerOptions { get; private set; } = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Converters =
            {
                new EnumMemberConverter<PresenceStatus>(),
                new EnumMemberConverter<TeamMemberRole>(),
                new EnumMemberConverter<OAuth2Scope>(),
                new EnumMemberConverter<Language>(),
                new SnowflakeConverter(),
            }
        };
        internal DiscordClientConfig ClientConfig { get; init; }
        internal static Logger Logger { get; private set; } = default!;

        private ResumeConnInfos _resumeConnInfos = ResumeConnInfos.EmptyConnInfos;
        private readonly HttpClient _httpClient;
        private ClientWebSocket _webSocket;

        private int? _lastSequenceNumber = null;

        #region Events
        public event Action<DiscordClient, DiscordGuild>? OnGuildCreate;
        public event Action<DiscordClient, ReadyEventArgs>? OnReady;

        #endregion

        #region Constructors
        public DiscordClient() : this(DiscordClientConfig.Default) { }

        public DiscordClient(DiscordClientConfig clientConfig)
        {
            ClientConfig = clientConfig;
            Logger = new Logger(ClientConfig.LogLevel);
            _webSocket = new ClientWebSocket();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://discord.com/api/v{ClientConfig.Version}/"),
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", ClientConfig.Token);
        }

        internal async Task RestartClientAsync()
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
            }

            _webSocket = new ClientWebSocket();
            _resumeConnInfos = ResumeConnInfos.EmptyConnInfos;
            _lastSequenceNumber = null;
            Guilds = [];

            await StartAsync();
        }

        #endregion

        /// <summary>
        /// The <c>first</c> method you have to call after instantiating the DiscordClient.
        /// It will try to connect to the Discord Gateway and start all processes that are needed to operate the bot.
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            try
            {
                Logger.LogInfo("Starting Discord client...");

                AppDomain.CurrentDomain.UnhandledException += (sender, ex) =>
                {
                    Logger.LogError(ex);
                };

                Uri gatewayUri = new($"wss://gateway.discord.gg/?v={ClientConfig.Version}&encoding=json");
                await _webSocket.ConnectAsync(gatewayUri, CancellationToken.None);

                _ = ReceiveMessagesAsync();
                ShardHandler shardHandler = new(_httpClient);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #region ReceivePayload

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
                    Logger.LogPayload(ConsoleColor.Cyan, message, "[RECEIVED]:");

                    JsonDocument jsonDocument = JsonDocument.Parse(message);
                    await HandleReceivedMessage(jsonDocument);

                    ClearMs(ms);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
            }

            Logger.LogDebug("Connection closed!");
        }

        private static void ClearMs(MemoryStream ms)
        {
            byte[] buffer = ms.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            ms.Position = 0;
            ms.SetLength(0);
        }

        #endregion

        internal void InvokeEvent(Events events, JsonElement jsonElement)
        {
            try
            {
                switch (events)
                {
                    case Events.READY:
                        HandleReadyEvent(jsonElement);
                        break;
                    case Events.GUILD_CREATE:
                        HandleGuildCreateEvent(jsonElement);
                        break;
                    default:
                        Logger.LogWarning($"Unhandled event: {events}.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #region HandleEvents

        internal void HandleReadyEvent(JsonElement jsonElement)
        {
            ReadyEventArgs readyEventArgs = jsonElement.GetProperty("d").Deserialize<ReadyEventArgs>(JsonSerializerOptions)!;
            OnReady?.Invoke(this, readyEventArgs);

            _resumeConnInfos = new ResumeConnInfos
            {
                SessionId = readyEventArgs.SessionId,
                ResumeGatewayUri = new Uri(readyEventArgs.ResumeGatewayUri)
            };
        }

        internal void HandleGuildCreateEvent(JsonElement jsonElement)
        {
            JsonElement data = jsonElement.GetProperty("d");
            IGuildCreateEventArgs guildCreateEventArgs = data.GetProperty("unavailable").GetBoolean()
                ? data.Deserialize<UnavailableGuildCreateEventArgs>(JsonSerializerOptions)!
                : data.Deserialize<GuildCreateEventArgs>(JsonSerializerOptions)!;

            GuildCreateEventArgs? guildCreate = guildCreateEventArgs.TryGetAvailableGuild();
            DiscordGuild discordGuild = guildCreate is null
                ? new DiscordGuild(guildCreateEventArgs.TryGetUnavailableGuild()!)
                : new DiscordGuild(guildCreate);

            OnGuildCreate?.Invoke(this, discordGuild);
        }

        #endregion

        private async Task HandleReceivedMessage(JsonDocument jsonDocument)
        {
            JsonElement message = jsonDocument.RootElement;
            OpCode opCode = message.GetOpCode();

            try
            {
                switch (opCode)
                {
                    case OpCode.Dispatch:
                        _lastSequenceNumber = HandleDiscordPayload.HandleDispatch(message, this);
                        break;
                    case OpCode.Hello:
                        Logger.LogInfo("Received Hello message.");
                        await HandleDiscordPayload.HandleHelloEvent(message, this, _resumeConnInfos, _lastSequenceNumber);
                        break;
                    case OpCode.HeartbeatAck:
                        Logger.LogDebug("Heartbeat acknowledged.");
                        break;
                    case OpCode.Reconnect:
                        Logger.LogWarning("Reconnect requested.");
                        await HandleDiscordPayload.HandleResumeConnAsync(this, _webSocket, _resumeConnInfos);
                        break;
                    case OpCode.InvalidSession:
                        Logger.LogWarning("Invalid session.");
                        await HandleDiscordPayload.HandleSessionInvalidAsync(this, message, _resumeConnInfos, _webSocket);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #region SendPayload
        internal async Task SendPayloadWssAsync(object payload)
        {
            string jsonStr = JsonSerializer.Serialize(payload, JsonSerializerOptions);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonStr);

            await _webSocket.SendAsync(jsonBytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        internal async Task SendIdentifyAsync()
        {
            Logger.LogInfo("Sending Identify payload");

            const string clientId = "DiscordBotLibrary";
            var identifyPayload = new
            {
                op = OpCode.Identify,
                d = new
                {
                    token = ClientConfig.Token,
                    properties = new
                    {
                        os = Environment.OSVersion.Platform.ToString(),
                        browser = clientId,
                        device = clientId,
                    },
                    intents = ClientConfig.Intents,
                }
            };

            await SendPayloadWssAsync(identifyPayload);
        }

        public async Task SendHeartbeatsAsync(int interval)
        {
            while (_webSocket.State == WebSocketState.Open)
            {
                Logger.LogDebug($"Sending heartbeat. Sequence: {_lastSequenceNumber}");
                var heartbeatPayload = new
                {
                    op = OpCode.Heartbeat,
                    d = _lastSequenceNumber
                };

                await SendPayloadWssAsync(heartbeatPayload);
                await Task.Delay(interval);
            }
        }
        #endregion
    }
}
