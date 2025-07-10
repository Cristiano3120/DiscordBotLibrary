using System.Collections.Concurrent;
using System.Diagnostics;
using DiscordBotLibrary.ChannelResources.ChannelEnums;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the main class for interacting with the Discord API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public sealed class DiscordClient
    {
        #region Internal/Private fields

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
        internal ConcurrentDictionary<ulong, DiscordGuild> InternalGuilds { get; private set; } = [];
        internal RestApiLimiter RestApiLimiter { get; private set; } = default!;
        internal static Logger Logger { get; private set; } = default!;

        private static ServiceProvider _serviceProvider = default!;
        private readonly VoiceChannelHandler _voiceChannelHandler;
        private readonly DiscordClientConfig _clientConfig;
        private readonly ShardHandler _shardHandler;

        #endregion

        #region External fields
        public IReadOnlyDictionary<ulong, DiscordGuild> Guilds => InternalGuilds;
        public IReadOnlyDictionary<ulong, VoiceChannelConn> VoiceConnections
            => _voiceChannelHandler.GetVoiceConns();

        #endregion

        #region Events

        public event Action<DiscordClient, VoiceState>? OnVoiceStateUpdate;
        public event Action<DiscordClient, DiscordGuild>? OnGuildCreate;
        public event Action<DiscordClient, Presence>? OnPresenceUpdate;

        #region ChannelEvents
        public event Action<DiscordClient, Channel>? OnChannelCreated;
        public event Action<DiscordClient, Channel>? OnChannelDeleted;
        public event Action<DiscordClient, Channel>? OnChannelUpdated;

        public event Action<DiscordClient, ChannelPins>? OnChannelPinsUpdate;
        #endregion

        public event Action<DiscordClient, ReadyEventArgs>? OnReady;
        public event Action<DiscordClient, IReadOnlyDictionary<ulong, DiscordGuild>>? OnGuildsReceived;
        #endregion

        #region Constructors
        public DiscordClient() : this(DiscordClientConfig.Default, new()) { }

        public DiscordClient(DiscordClientConfig clientConfig, LoggerConfig loggerConfig)
        {
            _clientConfig = clientConfig;
            Logger = new Logger(loggerConfig);
            _shardHandler = new(this);
            _voiceChannelHandler = new VoiceChannelHandler(_shardHandler);
            RestApiLimiter = new(_clientConfig);

            ServiceCollection services = new();
            services.AddSingleton(this);
            _serviceProvider = services.BuildServiceProvider();
        }

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

        /// <summary>
        /// Give the internal calling method the <see cref="DebuggerStepThroughAttribute"/>
        /// </summary>
        /// <param name="methodSignature"></param>
        /// <param name="neededIntents"></param>
        /// <exception cref="MissingIntentException"></exception>
        [DebuggerStepThrough]
        private void IntentChecker(CallerInfos callerInfos, params Intents[] neededIntents)
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
                throw new MissingIntentException(neededIntents, missingIntents, callerInfos);
            }
        }

        /// <summary>
        /// Sets the bot's presence and activity in Discord.
        /// Bots can only set the activity propertys: <c> name, state, type, and url</c>/>
        /// </summary>
        /// <param name="presenceUpdate"></param>
        /// <returns></returns>
        public async Task UpdatePresenceAsync(SelfPresenceUpdate presenceUpdate)
            => await _shardHandler.SendGlobalWebSocketMessageAsync<SelfPresenceUpdate>(new(OpCode.PresenceUpdate, presenceUpdate));
        
        public async Task<bool> LeaveGuildAsync(ulong guildId)
        {
            Logger.Log(LogLevel.Info, $"Left guild: {InternalGuilds[guildId].Name}({guildId})");
            string endpoint = RestApiEndpoints.GetGuildEndpoint(guildId, ChannelEndpoint.Delete);
            return await RestApiLimiter.DeleteAsync(endpoint, CallerInfos.Create());
        }

        #region VoiceChannelHandling

        /// <summary>
        /// Connects the bot to a voice channel in a specific guild.
        /// </summary>
        /// <returns></returns>
        internal async Task JoinVoiceChannelAsync(ulong guildId, ulong channelId, bool selfDeaf = false, bool selfMute = false)
            => await _voiceChannelHandler.ConnectToVcAsync(guildId, channelId, selfDeaf, selfMute);
        
        /// <summary>
        /// Disconnects the bot from the voice channel in a specific guild.
        /// </summary>
        /// <returns></returns>
        internal async Task LeaveVoiceChannelAsync(ulong guildId)
            => await _voiceChannelHandler.DisconnectFromVcAsync(guildId);

        internal void ReceivedVoiceServerUpdate(VoiceServerUpdate voiceServerUpdate)
            => _voiceChannelHandler!.ReceivedVoiceServerUpdate(voiceServerUpdate);

        #endregion

        #region RequestGuildMembers

        /// <summary>
        /// Requests guild members by user IDs. Max 100 IDs per request. Requires the Guild Members intent.
        /// </summary>
        /// <param name="guildId">The guild to request members from.</param>
        /// <param name="userIds">A list of user IDs to request.</param>
        /// <exception cref="ArgumentException">Thrown if more than 100 user IDs are passed.</exception>
        [DebuggerStepThrough]
        public async Task<List<GuildMember>?> GetGuildMembersByIdAsync(ulong guildId, ulong[] userIds, bool presences, bool cacheFetchedMembers = true)
        {
            HashSet<Intents> neededIntents = [Intents.GuildMembers];
            if (presences)
            {
                neededIntents.Add(Intents.GuildPresences);
            }
            IntentChecker(CallerInfos.Create(), [.. neededIntents]);

            if (userIds.Length == 0)
            {
                Logger.LogError("No user IDs provided for RequestGuildMembersByIdAsync().", CallerInfos.Create());
                return null;
            }

            if (userIds.Length > 100)
                userIds = [.. userIds.Take(100)];

            List<GuildMember> members = new();
            HashSet<ulong> nonCachedUsers = [];

            foreach (ulong id in userIds)
            {
                if (InternalGuilds.TryGetValue(guildId, out DiscordGuild? guild))
                {
                    GuildMember? found = guild.Members.FirstOrDefault(x => x?.User?.Id == id);
                    if (found is not null)
                    {
                        members.Add(found);
                        continue;
                    }
                }

                nonCachedUsers.Add(id);
            }

            if (nonCachedUsers.Count == 0)
                return members;

            TaskCreationOptions taskCreationOptions = TaskCreationOptions.RunContinuationsAsynchronously;
            TaskCompletionSource<List<GuildMember>> taskCompletionSource = new(taskCreationOptions);

            RequestGuildMembers requestGuildMembers = new()
            {
                GuildId = guildId,
                UserIds = [.. nonCachedUsers],
                Presences = presences,
                Limit = userIds.Length,
            };

            RequestGuildMembersCache requestGuildMembersCache = new(taskCompletionSource, requestGuildMembers, cacheFetchedMembers);
            await _shardHandler.RequestGuildMembersAsync(requestGuildMembersCache);

            List<GuildMember> fetchedMembers = await taskCompletionSource.Task;
            members.AddRange(fetchedMembers);

            return members;
        }

        /// <summary>
        /// Requests a single guild member by user ID. Requires the Guild Members intent.
        /// </summary>
        /// <param name="guildId">The guild to request the member from.</param>
        /// <param name="userId">The ID of the user to request.</param>
        [DebuggerStepThrough]
        public async Task GetGuildMemberByIdAsync(ulong guildId, ulong userId, bool presences, bool cacheFetchedMembers = true)
        {
            HashSet<Intents> neededIntents = [Intents.GuildMembers];
            if (presences)
            {
                neededIntents.Add(Intents.GuildPresences);
            }
            IntentChecker(CallerInfos.Create(), [.. neededIntents]);

            await GetGuildMembersByIdAsync(guildId, [userId], presences, cacheFetchedMembers);
        }

        /// <summary>
        /// Requests guild members whose usernames start with the given prefix. Useful for search/autocomplete.
        /// </summary>
        /// <param name="guildId">The ID of the guild.</param>
        /// <param name="prefix">The username prefix to search for (e.g. "Cra").</param>
        /// <param name="limit">Maximum number of users to return (0(gets all)–100).</param>
        /// <param name="presences">Whether to include presence data. For exapmle if the user is online etc</param>
        /// <exception cref="ArgumentOutOfRangeException">If limit is not between 1 and 100.</exception>
        [DebuggerStepThrough]
        public async Task<List<GuildMember>> GetGuildMembersByPrefixAsync(ulong guildId, string prefix, bool presences, int limit = 0, bool cacheFetchedMembers = true)
        {
            HashSet<Intents> neededIntents = [Intents.GuildMembers];
            if (presences)
            {
                neededIntents.Add(Intents.GuildPresences);
            }
            IntentChecker(CallerInfos.Create(), [.. neededIntents]);

            if (limit < 1)
                limit = 1;
            else if (limit > 100)
                limit = 100;

            RequestGuildMembers requestGuildMembers = new()
            {
                GuildId = guildId,
                Query = prefix,
                Limit = limit,
                Presences = presences,
            };

            TaskCompletionSource<List<GuildMember>> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
            RequestGuildMembersCache requestGuildMembersCache = new(taskCompletionSource, requestGuildMembers, cacheFetchedMembers);

            await _shardHandler.RequestGuildMembersAsync(requestGuildMembersCache);
            return await taskCompletionSource.Task;
        }

        /// <summary>
        /// Requests all guild members in a guild
        /// </summary>
        /// <param name="guildId"></param>
        /// <param name="presences"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public async Task<List<GuildMember>> GetAllGuildMembersAsync(ulong guildId, bool presences, bool cacheFetchedMembers = true)
        {
            HashSet<Intents> neededIntents = [Intents.GuildMembers];
            if (presences)
            {
                neededIntents.Add(Intents.GuildPresences);
            }
            IntentChecker(CallerInfos.Create(), [.. neededIntents]);

            RequestGuildMembers requestGuildMembers = new()
            {
                GuildId = guildId,
                Presences = presences,
                Query = "",
                Limit = 0,
            };

            TaskCompletionSource<List<GuildMember>> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
            RequestGuildMembersCache requestGuildMembersCache = new(taskCompletionSource, requestGuildMembers, cacheFetchedMembers);

            await _shardHandler.RequestGuildMembersAsync(requestGuildMembersCache);
            return await taskCompletionSource.Task;
        }

        #endregion

        #region RequestSoundboardSounds

        public async Task<SoundboardSound[]> GetSoundboardSoundsAsync(ulong guildId)
        {
            Dictionary<ulong, SoundboardSound[]> dict = await _shardHandler.RequestSoundboardSoundsAsync([guildId]);
            return dict.First().Value;
        }

        public async Task<Dictionary<ulong, SoundboardSound[]>> GetSoundboardSoundsAsync(ulong[] guildIds)
            => await _shardHandler.RequestSoundboardSoundsAsync(guildIds);

        #endregion

        #region InvokeEvents
        internal void InvokeOnGuildCreate(DiscordGuild guild)
            => OnGuildCreate?.Invoke(this, guild);

        internal void InvokeOnPresenceUpdate(Presence presence)
            => OnPresenceUpdate?.Invoke(this, presence);

        internal void InvokeOnVoiceStateUpdate(VoiceState voiceState)
            => OnVoiceStateUpdate?.Invoke(this, voiceState);

        internal void InvokeOnChannelCreated(Channel channel)
            => OnChannelCreated?.Invoke(this, channel);

        internal void InvokeOnChannelDeleted(Channel channel)
            => OnChannelDeleted?.Invoke(this, channel);

        internal void InvokeOnChannelUpdated(Channel channel)
            => OnChannelUpdated?.Invoke(this, channel);

        internal void InvokeOnChannelPinsUpdate(ChannelPins channelPins)
            => OnChannelPinsUpdate?.Invoke(this, channelPins);
        #endregion

        #region GetMethods

        internal static DiscordClient GetDiscordClient()
            => _serviceProvider.GetRequiredService<DiscordClient>();

        internal int GetTotalShardCount()
            => _shardHandler.TotalShards;

        /// <summary>
        /// If this method returns null one of the params is invalid
        /// </summary>
        public DiscordGuild? GetGuild(ulong guildId)
            => InternalGuilds.TryGetValue(guildId, out DiscordGuild? guild)
                ? guild
                : null;

        /// <summary>
        /// If this method returns null one of the params is invalid
        /// </summary>
        public Channel? GetChannel(ulong guildId, ulong channelId)
            => InternalGuilds.TryGetValue(guildId, out DiscordGuild? guild)
                ? guild.GetChannel(channelId)
                : null;

        #endregion
    }
}
