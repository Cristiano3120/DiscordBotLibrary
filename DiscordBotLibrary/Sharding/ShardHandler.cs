using Microsoft.Extensions.DependencyInjection;

namespace DiscordBotLibrary.Sharding
{
    internal static class ShardHandler
    {
        public static ReadyEventArgs? ReadyEventArgs { get; set; } = new();
        public static int TotalShards { get; private set; }

        private static HashSet<int> _shardIds = new();
        private static Shard[] _shards = [];

        public static async Task Start(HttpClient httpClient)
        {
            await InitAndConnectShardsAsync(httpClient);
        }

        #region StartShards
        private static async Task InitAndConnectShardsAsync(HttpClient httpClient)
        {
            GatewayShardingInfo gatewayShardingInfo = await FetchGatewayShardingInfoAsync(httpClient);
            await StartShardsAsync(gatewayShardingInfo);
        }

        private static async Task<GatewayShardingInfo> FetchGatewayShardingInfoAsync(HttpClient httpClient)
        {
            DiscordClient.Logger.LogDebug("Fetching sharding information from Discord API");

            string response = await httpClient.GetStringAsync("https://discord.com/api/v10/gateway/bot");
            GatewayShardingInfo gatewayShardingInfo = JsonSerializer.Deserialize<GatewayShardingInfo>(response, DiscordClient.JsonSerializerOptions);

            TimeSpan waitTime = TimeSpan.FromMilliseconds(gatewayShardingInfo.SessionStartLimit.ResetAfter);
            DateTime resumeTime = DateTime.UtcNow + waitTime;

            DiscordClient.Logger.LogInfo($"Used {gatewayShardingInfo.SessionStartLimit.Remaining} out of " +
                $"{gatewayShardingInfo.SessionStartLimit.Total} logins");
            DiscordClient.Logger.LogInfo($"Remaining logins will be reseted at: {resumeTime}");

            TotalShards = gatewayShardingInfo.Shards;
            _shards = new Shard[gatewayShardingInfo.Shards];
            if (gatewayShardingInfo.Shards > gatewayShardingInfo.SessionStartLimit.Remaining)
            {
                DiscordClient.Logger.LogDebug($"Login limit reached. Waiting until {resumeTime}");
                await Task.Delay(waitTime);
            }

            return gatewayShardingInfo;
        }

        private static async Task StartShardsAsync(GatewayShardingInfo gatewayShardingInfo)
        {
            List<List<int>> shards = [];
            for (int i = 0; i < gatewayShardingInfo.SessionStartLimit.MaxConcurrency; i++)
            {
                shards.Add([]);
            }

            for (int shardId = 0; shardId < gatewayShardingInfo.Shards; shardId++)
            {
                int bucket = shardId % gatewayShardingInfo.SessionStartLimit.MaxConcurrency;
                shards[bucket].Add(shardId);
            }

            int maxRounds = shards.Max(x => x.Count);
            for (int round = 0; round < maxRounds; round++)
            {
                List<Task> tasks = [];

                for (int bucket = 0; bucket < gatewayShardingInfo.SessionStartLimit.MaxConcurrency; bucket++)
                {
                    if (round < shards[bucket].Count)
                    {
                        int shardId = shards[bucket][round];
                        tasks.Add(StartShardAsync(shardId));
                    }
                }

                DiscordClient.Logger.LogDebug($"Started {tasks.Count + _shards.Where(x => x is not null).Count()} out of {gatewayShardingInfo.Shards} shards");
                await Task.WhenAll(tasks);

                if (round < maxRounds - 1)
                {
                    DiscordClient.Logger.LogDebug($"Waiting 5 seconds till the next shards will be started");
                    await Task.Delay(5000);
                }
            }
        }

        private static async Task StartShardAsync(int shardId)
        {
            Shard shard = new(shardId);
            await shard.StartShardAsync();
            _shards[shardId] = shard;
        }

        #endregion

        #region WssRequests

        public static async Task RequestGuildMembersAsync(RequestGuildMembersCache requestGuildMembersCache)
        {
            int shardId = GetResponsibleShardId(requestGuildMembersCache.RequestGuildMembers.GuildId);
            await _shards[shardId].SendGuildMemberRequestAsync(requestGuildMembersCache);
        }

        public static async Task<Dictionary<ulong, SoundboardSound[]>> RequestSoundboardSoundsAsync(ulong[] guildIds)
        {
            if (guildIds.Length == 0)
                throw new ArgumentException("Can't request soundboard sounds with an empty array.");

            Dictionary<int, ulong[]> guildsPerShard = guildIds
                .GroupBy(GetResponsibleShardId)
                .ToDictionary(g => g.Key, g => g.ToArray());

            List<ulong> allGuildIdsInOrder = new();

            List<Task<SoundboardSound[]>> tasks = new();
            foreach ((int shardId, ulong[] shardGuildIds) in guildsPerShard)
            {
                RequestSoundboardSounds requestSoundboardSounds = new(shardGuildIds);
                foreach (ulong guildId in shardGuildIds)
                {
                    requestSoundboardSounds.TaskCompletionSources
                        .TryGetValue(guildId, out TaskCompletionSource<SoundboardSound[]>? tcs);

                    if (tcs == null)
                        throw new InvalidOperationException($"No TaskCompletionSource for guildId {guildId}");

                    tasks.Add(tcs.Task);
                    allGuildIdsInOrder.Add(guildId);
                }

                _ = _shards[shardId].SendSoundboardRequestAsync(requestSoundboardSounds);
            }

            SoundboardSound[][] soundboardSounds = await Task.WhenAll(tasks);

            Dictionary<ulong, SoundboardSound[]> result = new Dictionary<ulong, SoundboardSound[]>(allGuildIdsInOrder.Count);
            for (int i = 0; i < allGuildIdsInOrder.Count; i++)
            {
                result[allGuildIdsInOrder[i]] = soundboardSounds[i];
            }

            return result;
        }


        #endregion

        public static void ShardReady(ShardReadyEventArgs shardReadyEventArgs)
        {
            _shardIds.Add(shardReadyEventArgs.Shard![0]);
            ReadyEventArgs!.Guilds = [.. ReadyEventArgs.Guilds, .. shardReadyEventArgs.Guilds];

            if (_shardIds.Count == TotalShards)
            {
                DiscordClient client = DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>();
                _ = client.ShardsReady(ReadyEventArgs);
                _shardIds = null!;
            }
        }

        public static async Task SendShardSpecificMessageAsync(ulong guildId, object payload)
        {
            int shardID = GetResponsibleShardId(guildId);
            await _shards[shardID].SendPayloadWssAsync(payload);
        }

        public static async Task SendGlobalWebSocketMessageAsync(object payload)
        {
            foreach (Shard shard in _shards)
            {
                await shard.SendPayloadWssAsync(payload);
            }
        }

        private static int GetResponsibleShardId(ulong guildId)
            => (int)((guildId >> 22) % (ulong)TotalShards);
    }
}