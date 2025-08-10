using Microsoft.Extensions.DependencyInjection;

namespace DiscordBotLibrary.DiscordClientResources
{
    public sealed partial class DiscordClient
    {
        internal static DiscordClient GetDiscordClient()
            => _serviceProvider.GetRequiredService<DiscordClient>();

        public VoiceChannelHandler GetVoiceChannelHandler()
            => VoiceChannelHandler;

        internal int GetTotalShardCount()
            => _shardHandler.TotalShards;

        public async Task<GuildInvite?> GetInviteAsync(string inviteCode)
        {
            string endpoint = RestApiEndpoints.GetInviteEndpoint(inviteCode, HttpRequestType.Get);
            return await RestApiLimiter.GetAsync<GuildInvite>(endpoint, CallerInfos.Create());
        }

        #region GetGuild

        /// <summary>
        /// Searches for the Guild locally in the cache. <br></br>
        /// If it´s not here and you are sure it exists use the <see cref="GetGuildAsync(ulong)"/> method
        /// </summary>
        public DiscordGuild? GetGuild(ulong guildId)
            => InternalGuilds.TryGetValue(guildId, out DiscordGuild? guild)
                ? guild
                : null;

        /// <summary>
        /// Searches for the Guild locally in the cache. <br></br>
        /// If it´s not here and you are sure it exists use the <see cref="GetGuildAsync(ulong)"/> method
        /// </summary>
        public DiscordGuild? GetGuild(Func<DiscordGuild, bool> predicate)
            => InternalGuilds.Values.FirstOrDefault(predicate);

        /// <summary>
        /// Searches for the Guild locally in the cache<br></br>
        /// If it´s not found locally: Requests the Guild from discords servers
        /// </summary>
        public async Task<DiscordGuild?> GetGuildAsync(ulong guildId)
        {
            DiscordGuild? discordGuild = GetGuild(guildId);
            return discordGuild is null
                ? await RestApiLimiter.GetAsync<DiscordGuild>(RestApiEndpoints.GetGuildEndpoint(guildId, GuildSubresource.None), CallerInfos.Create())
                : discordGuild;
        }

        #endregion

        #region GetSoundboardSounds

        public async Task<SoundboardSound[]> GetSoundboardSoundsAsync(ulong guildId)
        {
            Dictionary<ulong, SoundboardSound[]> dict = await _shardHandler.RequestSoundboardSoundsAsync([guildId]);
            return dict.First().Value;
        }

        public async Task<Dictionary<ulong, SoundboardSound[]>> GetSoundboardSoundsAsync(ulong[] guildIds)
            => await _shardHandler.RequestSoundboardSoundsAsync(guildIds);

        #endregion

        #region RequestGuildMembers

        /// <summary>
        /// Requests guild members by user IDs. Max 100 IDs per request. Requires the Guild Members intent.
        /// </summary>
        /// <param name="guildId">The guild to request members from.</param>
        /// <param name="userIds">A list of user IDs to request.</param>
        /// <exception cref="ArgumentException">Thrown if more than 100 user IDs are passed.</exception>
        public async Task<List<GuildMember>?> GetGuildMembersByIdAsync(ulong guildId, ulong[] userIds, bool presences, bool cacheFetchedMembers = true)
        {
            HashSet<Intents> neededIntents = [Intents.GuildMembers];
            if (presences)
            {
                neededIntents.Add(Intents.GuildPresences);
            }

            if (!IntentChecker(CallerInfos.Create(), [.. neededIntents]))
            {
                return null;
            }    

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
        public async Task GetGuildMemberByIdAsync(ulong guildId, ulong userId, bool presences, bool cacheFetchedMembers = true)
        {
            HashSet<Intents> neededIntents = [Intents.GuildMembers];
            if (presences)
            {
                neededIntents.Add(Intents.GuildPresences);
            }
            
            if (!IntentChecker(CallerInfos.Create(), [.. neededIntents]))
            {
                 return;
            }

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
        public async Task<List<GuildMember>?> GetGuildMembersByPrefixAsync(ulong guildId, string prefix, bool presences, int limit = 0, bool cacheFetchedMembers = true)
        {
            HashSet<Intents> neededIntents = [Intents.GuildMembers];
            if (presences)
            {
                neededIntents.Add(Intents.GuildPresences);
            }
            
            if (!IntentChecker(CallerInfos.Create(), [.. neededIntents]))
            {
                return null;
            }

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
        public async Task<List<GuildMember>?> GetAllGuildMembersAsync(ulong guildId, bool presences, bool cacheFetchedMembers = true)
        {
            HashSet<Intents> neededIntents = [Intents.GuildMembers];
            if (presences)
            {
                neededIntents.Add(Intents.GuildPresences);
            }
            
            if (!IntentChecker(CallerInfos.Create(), [.. neededIntents]))
            {
                return null;
            }

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
    }
}
