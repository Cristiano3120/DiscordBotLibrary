using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Headers;
using DiscordBotLibrary.Sharding;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the main class for interacting with the Discord API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public sealed class DiscordClient
    {
        #region Internal/Private fields
        internal static ServiceProvider ServiceProvider { get; private set; } = default!;
        internal static JsonSerializerOptions JsonSerializerOptions { get; private set; } = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IncludeFields = true,
            WriteIndented = true,
            Converters =
            {
                new EnumMemberConverter<PresenceStatus>(),
                new EnumMemberConverter<TeamMemberRole>(),
                new EnumMemberConverter<OAuth2Scope>(),
                new EnumMemberConverter<Language>(),
                new ActivityButtonsConverter(),
                new SnowflakeConverter(),
            }
        };
        internal static DiscordClientConfig ClientConfig { get; private set; } = default!;
        internal static Logger Logger { get; private set; } = default!;
        internal ConcurrentDictionary<ulong, DiscordGuild> InternalGuilds { get; private set; } = [];

        private VoiceChannelHandler? _voiceChannelHandler;
        private readonly HttpClient _httpClient;

        #endregion

        #region External fields
        public IReadOnlyDictionary<ulong, DiscordGuild> Guilds => InternalGuilds;
        public IReadOnlyDictionary<ulong, VoiceChannelConn> VoiceConnections
            => _voiceChannelHandler?.VoiceConnections.ToDictionary(kvp => kvp.Key, kvp => (VoiceChannelConn)kvp.Value)
            ?? [];

        #endregion

        #region Events

        public event Action<DiscordClient, VoiceState>? OnVoiceStateUpdate;
        public event Action<DiscordClient, DiscordGuild>? OnGuildCreate;
        public event Action<DiscordClient, Presence>? OnPresenceUpdate;

        #region ChannelEvents
        public event Action<DiscordClient, Channel>? OnChannelCreated;
        public event Action<DiscordClient, Channel>? OnChannelDeleted;
        public event Action<DiscordClient, Channel>? OnChannelUpdated;
        #endregion
        public event Action<DiscordClient, ReadyEventArgs>? OnReady;   

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
        /// It will try to connect to the Discord Gateway and start all processes that are needed to operate the bot
        /// </summary>
        /// <returns>An Logger that is recommended to use</returns>
        public async Task<Logger> Start()
        {
            try
            {
                Logger.LogInfo("Starting Discord client...");

                AppDomain.CurrentDomain.UnhandledException += (sender, ex) =>
                {
                    Logger.LogError(ex);
                };

                await ShardHandler.Start(_httpClient);
                return Logger;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw new Exception("Failed to start Discord client");
            }
        }

        internal async Task ShardsReady(ReadyEventArgs? args)
        {
            while (true)
            {
                if (args?.Guilds.Length == InternalGuilds.Count)
                {
                    break;
                }

                await Task.Delay(100);
            }

            OnReady?.Invoke(this, args!);
            ShardHandler.ReadyEventArgs = null;
        }

        /// <summary>
        /// Give the internal calling method the <see cref="DebuggerStepThroughAttribute"/>
        /// </summary>
        /// <param name="methodSignature"></param>
        /// <param name="neededIntents"></param>
        /// <exception cref="MissingIntentException"></exception>
        [DebuggerStepThrough]
        private void IntentChecker(string methodSignature, params Intents[] neededIntents)
        {
            HashSet<Intents> missingIntents = [];
            foreach (Intents intent in neededIntents)
            {
                if (!ClientConfig.Intents.HasFlag(intent))
                {
                    missingIntents.Add(intent);
                }
            }

            if (missingIntents.Count > 0)
            {
                throw new MissingIntentException(neededIntents, missingIntents, methodSignature);
            }
        }

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

        #region VoiceChannelHandling

        /// <summary>
        /// Connects the bot to a voice channel in a specific guild.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public async Task ConnectToVcAsync(ulong guildId, ulong channelId, bool selfDeaf = false, bool selfMute = false)
        {
            if (!InternalGuilds.TryGetValue(guildId, out DiscordGuild? discordGuild))
            {
                throw new ArgumentException($"No guild found with that guild id", nameof(guildId));
            }

            if (!discordGuild.CheckIfChannelIsVc(channelId))
            {
                throw new ArgumentException($"The channel either doesnt exist in this guild or is not a voice channel");
            }

            _voiceChannelHandler ??= new VoiceChannelHandler();
            await _voiceChannelHandler.ConnectToVcAsync(guildId, channelId, selfDeaf, selfMute);
        }

        /// <summary>
        /// Disconnects the bot from the voice channel in a specific guild.
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectFromVcAsync(ulong guildId)
        {
            if (_voiceChannelHandler != null)
                await _voiceChannelHandler.DisconnectFromVcAsync(guildId);
        }

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
            IntentChecker("RequestGuildMembersByIdAsync(ulong guildId, ulong[] userIds, bool presences)", [.. neededIntents]);

            if (userIds.Length == 0)
            {
                Logger.LogError("No user IDs provided for RequestGuildMembersByIdAsync().");
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
            await ShardHandler.RequestGuildMembersAsync(requestGuildMembersCache);

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
            IntentChecker("RequestGuildMemberByIdAsync(ulong guildId, ulong userId, bool presences)", [.. neededIntents]);

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
            IntentChecker("RequestGuildMembersByPrefixAsync(ulong guildId, string prefix, bool presences, int limit = 0)", [.. neededIntents]);

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

            await ShardHandler.RequestGuildMembersAsync(requestGuildMembersCache);
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
            IntentChecker("RequestAllGuildMembersAsync(ulong guildId, bool presences)", [.. neededIntents]);

            RequestGuildMembers requestGuildMembers = new()
            {
                GuildId = guildId,
                Presences = presences,
                Query = "",
                Limit = 0,
            };

            TaskCompletionSource<List<GuildMember>> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
            RequestGuildMembersCache requestGuildMembersCache = new(taskCompletionSource, requestGuildMembers, cacheFetchedMembers);

            await ShardHandler.RequestGuildMembersAsync(requestGuildMembersCache);
            return await taskCompletionSource.Task;
        }

        #endregion

        #region RequestSoundboardSounds

        public async Task<SoundboardSound[]> GetSoundboardSoundsAsync(ulong guildId)
        {
            Dictionary<ulong, SoundboardSound[]> dict = await ShardHandler.RequestSoundboardSoundsAsync([guildId]);
            return dict.First().Value;
        }

        public async Task<Dictionary<ulong, SoundboardSound[]>> GetSoundboardSoundsAsync(ulong[] guildIds)
            => await ShardHandler.RequestSoundboardSoundsAsync(guildIds);

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
        #endregion

        #region GetMethods
        public DiscordGuild? GetGuild(ulong guildId)
            => InternalGuilds.TryGetValue(guildId, out DiscordGuild? guild)
                ? guild
                : null;

        public Channel? GetChannel(ulong guildId, ulong channelId)
            => InternalGuilds.TryGetValue(guildId, out DiscordGuild? guild)
                ? guild.GetChannel(channelId)
                : null;

        #endregion
    }
}
