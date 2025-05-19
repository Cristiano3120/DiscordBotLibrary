using Microsoft.Extensions.DependencyInjection;
using DiscordBotLibrary.Sharding;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Concurrent;


namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the main class for interacting with the Discord API.
    /// </summary>
    public sealed class DiscordClient
    {
        #region Internal/Private fields
        internal static ServiceProvider ServiceProvider { get; private set; } = default!;
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
        internal static DiscordClientConfig ClientConfig { get; private set; } = default!;
        internal static Logger Logger { get; private set; } = default!;
        internal ConcurrentDictionary<ulong, DiscordGuild> InternalGuilds { get; private set; } = [];

        private readonly HttpClient _httpClient;

        #endregion

        #region External fields
        public IReadOnlyDictionary<ulong, DiscordGuild> Guilds => InternalGuilds;

        #region Events
        public event Action<DiscordClient, PresenceUpdate>? OnPresenceUpdate;
        public event Action<DiscordClient, DiscordGuild>? OnGuildCreate;
        public event Action<DiscordClient, ReadyEventArgs>? OnReady;

        #endregion
        #endregion

        #region Constructors
        public DiscordClient() : this(DiscordClientConfig.Default) { }

        public DiscordClient(DiscordClientConfig clientConfig)
        {
            ClientConfig = clientConfig;
            Logger = new Logger(ClientConfig.LogLevel);

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://discord.com/api/v{ClientConfig.Version}/"),
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", ClientConfig.Token);

            ServiceCollection services = new();
            services.AddSingleton(this);
            ServiceProvider = services.BuildServiceProvider();
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

                ShardHandler.Start(_httpClient);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        #region WebSocket Send 

        /// <summary>
        /// Sets the bot's presence and activity in Discord.
        /// Bots can only set the activity propertys: <c> name, state, type, and url</c>/>
        /// </summary>
        /// <param name="presenceUpdate"></param>
        /// <returns></returns>
        public async Task UpdatePresence(SelfPresenceUpdate presenceUpdate)
        {
            var payload = new
            {
                op = OpCode.PresenceUpdate,
                d = presenceUpdate
            };

            await ShardHandler.SendGlobalWebSocketMessageAsync(payload);
        }
        #endregion

        #region InvokeEvents
        internal void InvokeOnReady(ReadyEventArgs args)
           => OnReady?.Invoke(this, args);

        internal void InvokeOnGuildCreate(DiscordGuild guild)
            => OnGuildCreate?.Invoke(this, guild);

        internal void InvokeOnPresenceUpdate(PresenceUpdate presence)
            => OnPresenceUpdate?.Invoke(this, presence);
        #endregion
    }
}
