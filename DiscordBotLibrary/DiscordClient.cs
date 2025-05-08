using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace DiscordBotLibrary
{
    public sealed class DiscordClient
    {
        // Musst machen das es extra klassen gibt die für den externen dev sind die dann alles kombinieren
        //Dazu zählt dann auch exxtra Property für Vc, für TextChat etc.
        public List<DiscordGuild> Guilds { get; init; } = [];
        internal Logger Logger { get; init; }

        private readonly DiscordClientConfig _clientConfig;
        private readonly ClientWebSocket _webSocket;
        private readonly HttpClient _httpClient;
        private ResumeConnInfos _resumeConnInfos;

        private int? _lastSequenceNumber = null;

        #region Events
        public delegate void ReadyEventHandler(DiscordClient discordClient, ReadyEventArgs args);
        public event ReadyEventHandler? OnReady;

        public delegate void GuildCreateEventHandler(DiscordClient discordClient, IGuildCreateEventArgs args);
        public event GuildCreateEventHandler? OnGuildCreate;

        #endregion

        #region Constructors
        public DiscordClient() : this(DiscordClientConfig.Default) { }

        public DiscordClient(DiscordClientConfig clientConfig)
        {
            _clientConfig = clientConfig;
            Logger = new Logger(_clientConfig.LogLevel);
            _webSocket = new ClientWebSocket();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://discord.com/api/v{_clientConfig}/"),
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", _clientConfig.Token);
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

                Uri gatewayUri = new($"wss://gateway.discord.gg/?v={_clientConfig.Version}&encoding=json");
                await _webSocket.ConnectAsync(gatewayUri, CancellationToken.None);

                _ = ReceiveMessagesAsync();
                await SendIdentifyAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error starting Discord client: {ex.Message}");
            }
        }

        #region ReceivePayload

        public async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[8192];
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
                    HandleReceivedMessage(jsonDocument);

                    ClearMs(ms);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Error receiving message: {ex.Message}");
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
            ReadyEventArgs readyEventArgs = jsonElement.GetProperty("d").Deserialize<ReadyEventArgs>()!;
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
                ? data.Deserialize<UnavailableGuildCreateEventArgs>()!
                : data.Deserialize<GuildCreateEventArgs>()!;

            GuildCreateEventArgs? guildCreate = guildCreateEventArgs.TryGetAvailableGuild();
            if (guildCreate != null)
            {
                Guilds.Add(new DiscordGuild(guildCreate));
            }
            else
            {
                Guilds.Add(new DiscordGuild(guildCreateEventArgs.TryGetUnavailableGuild()!));
            }

            OnGuildCreate?.Invoke(this, guildCreateEventArgs);
        }

        #endregion

        private void HandleReceivedMessage(JsonDocument jsonDocument)
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
                        HandleDiscordPayload.HandleHelloEvent(message, this);
                        break;
                    case OpCode.HeartbeatAck:
                        Logger.LogDebug("Heartbeat acknowledged.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error handling message: {ex.Message}");
            }
        }

        #region SendPayload
        internal async Task SendPayloadWssAsync(object payload)
        {
            string jsonStr = JsonSerializer.Serialize(payload);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonStr);

            await _webSocket.SendAsync(jsonBytes, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task SendIdentifyAsync()
        {
            Logger.LogInfo("Sending Identify payload");
            var identifyPayload = new
            {
                op = OpCode.Identify,
                d = new
                {
                    token = _clientConfig.Token,
                    properties = new
                    {
                        os = Environment.OSVersion.Platform.ToString(),
                        browser = "DiscordBotLibrary",
                        device = "DiscordBotLibrary"
                    },
                    intents = _clientConfig.Intents,
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
