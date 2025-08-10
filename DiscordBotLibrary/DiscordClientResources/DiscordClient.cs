using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace DiscordBotLibrary.DiscordClientResources
{
    /// <summary>
    /// Represents the main class for interacting with the Discord API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public sealed partial class DiscordClient
    {
        internal ConcurrentDictionary<ulong, DiscordGuild> InternalGuilds { get; private set; } = [];
        internal RestApiLimiter RestApiLimiter { get; private set; } = default!;
        internal static Logger Logger { get; private set; } = default!;
        internal VoiceChannelHandler VoiceChannelHandler { get; init; }

        private static ServiceProvider _serviceProvider = default!;       
        private readonly DiscordClientConfig _clientConfig;
        private readonly ShardHandler _shardHandler;

        #region Constructors
        public DiscordClient() : this(DiscordClientConfig.Default, new()) { }

        public DiscordClient(DiscordClientConfig clientConfig, LoggerConfig loggerConfig)
        {
            _clientConfig = clientConfig;
            Logger = new Logger(loggerConfig);
            _shardHandler = new(this);
            VoiceChannelHandler = new VoiceChannelHandler(_shardHandler);
            RestApiLimiter = new(_clientConfig);

            ServiceCollection services = new();
            services.AddSingleton(this);
            _serviceProvider = services.BuildServiceProvider();
        }

        #endregion

        #region JsonSerializer
        internal static JsonSerializerSettings ReceiveJsonSerializerOptions { get; private set; } = new()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true,
                    OverrideSpecifiedNames = false
                }
            },
            Converters =
            {
                new StringEnumConverter(),
                new ActivityButtonConverter(),
                new SnowflakeConverter(),
                new SnowflakeArrayConverter(),
                new OverwriteConverter(),
            }
        };

        internal static JsonSerializerSettings SendJsonSerializerSettings { get; } = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true,
                    OverrideSpecifiedNames = false
                }
            },
            Converters =
            {
                new ActivityButtonConverter(),
                new SnowflakeConverter(),
                new OptionalConverter(),
                new OverwriteConverter(),
            },
        };

        #endregion

        /// <summary>
        /// The <c>first</c> method you have to call after instantiating the DiscordClient.
        /// It will try to connect to the Discord Gateway and start all processes that are needed to operate the bot
        /// </summary>
        /// <returns>An Logger that is recommended to use</returns>
        public async Task<Logger> StartAsync()
        {
            try
            {
                Logger.Log(LogLevel.Info, "Starting Discord client...");

                await _shardHandler.StartAsync(_clientConfig);
                return Logger;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw new Exception("Failed to start Discord client");
            }
        }

        internal void ShardsReady(ShardReadyEventArgs args)
        {
            ReadyEventArgs? readyEventArgs = new()
            {
                Application = args.Application,
                DiscordUser = args.DiscordUser,
                Guilds = [..InternalGuilds.Values],
            };

            _ = CheckIfGuildsComplete();
            OnReady?.Invoke(this, readyEventArgs!);
        }

        private async Task CheckIfGuildsComplete()
        {
            using CancellationTokenSource cts = new(TimeSpan.FromSeconds(10));

            try
            {
                while (!cts.IsCancellationRequested)
                {
                    if (!InternalGuilds.IsEmpty && InternalGuilds.All(x => x.Value.Unavailable == false))
                    {
                        OnGuildsReceived?.Invoke(this, InternalGuilds);
                        return;
                    }

                    await Task.Delay(100, cts.Token);
                }

                OnGuildsReceived?.Invoke(this, InternalGuilds);
            }
            catch (TaskCanceledException)
            {
                OnGuildsReceived?.Invoke(this, InternalGuilds);
            }
        }

        public bool IntentChecker(CallerInfos callerInfos, params Intents[] neededIntents)
        {
            HashSet<Intents> missingIntents = [];
            foreach (Intents intent in neededIntents)
            {
                if (!_clientConfig.Intents.HasFlag(intent))
                {
                    missingIntents.Add(intent);
                }
            }

            if (missingIntents.Count > 0)
            {
                string errorMsg = $"The method[{callerInfos.CallerName}] in {callerInfos.FilePath} at line {callerInfos.LineNum} needs the following Intents: {string.Join(", ", neededIntents)} \n " +
                  $"You need to activate the following intents to be able to run this method: {string.Join(", ", missingIntents)}";
                Logger.CustomLog(ConsoleColor.Red, LogLevel.Error, errorMsg, "ERROR");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the bot's presence and activity in Discord.
        /// Bots can only set the activity propertys: <c> name, state, type, and url</c>/>
        /// </summary>
        /// <param name="presenceUpdate"></param>
        /// <returns></returns>
        public async Task UpdatePresenceAsync(SelfPresenceUpdate presenceUpdate)
            => await _shardHandler.SendGlobalWebSocketMessageAsync<SelfPresenceUpdate>(new(OpCode.PresenceUpdate, presenceUpdate));

        public async Task<bool> DeleteInviteAsync(string inviteCode)
        {
            string endpoint = RestApiEndpoints.GetInviteEndpoint(inviteCode, HttpRequestType.Delete);
            return await RestApiLimiter.DeleteAsync(endpoint, CallerInfos.Create());
        }
    }
}
