namespace DiscordBotLibrary.Sharding
{
    internal sealed class ShardHandler(DiscordClient discordClient)
    {
        public int TotalShards => _shards.Length;

        private readonly DiscordClient _discordClient = discordClient;
        /// <summary>
        /// If this is 0 all shards are ready. The starting value is the ammount of shards that should be started.
        /// </summary>
        private int? _shardsReady;
        private Shard[] _shards = [];
        
        public async Task StartAsync(DiscordClientConfig config)
        {
            await InitAndConnectShardsAsync(config);
        }

        #region StartShards
        private async Task InitAndConnectShardsAsync(DiscordClientConfig config)
        {
            GatewayShardingInfo gatewayShardingInfo = await FetchGatewayShardingInfoAsync();
            await StartShardsAsync(config, gatewayShardingInfo);
        }

        private async Task<GatewayShardingInfo> FetchGatewayShardingInfoAsync()
        {
            DiscordClient.Logger.Log(LogLevel.Debug, "Fetching sharding information from Discord API");

            string response = await _discordClient.RestApiLimiter.GetStringAsync("https://discord.com/api/v10/gateway/bot");
            GatewayShardingInfo gatewayShardingInfo = JsonConvert.DeserializeObject<GatewayShardingInfo>(response, DiscordClient.ReceiveJsonSerializerOptions);

            TimeSpan waitTime = TimeSpan.FromMilliseconds(gatewayShardingInfo.SessionStartLimit.ResetAfter);
            DateTime resumeTime = DateTime.UtcNow + waitTime;

            DiscordClient.Logger.Log(LogLevel.Info, $"Used {gatewayShardingInfo.SessionStartLimit.Remaining} out of " +
                $"{gatewayShardingInfo.SessionStartLimit.Total} logins");
            DiscordClient.Logger.Log(LogLevel.Info, $"Remaining logins will be reseted at: {resumeTime}");

            int shards = gatewayShardingInfo.Shards;
            _shards = new Shard[shards];
            _shardsReady = shards;

            if (gatewayShardingInfo.Shards > gatewayShardingInfo.SessionStartLimit.Remaining)
            {
                DiscordClient.Logger.Log(LogLevel.Debug, $"Login limit reached. Waiting until {resumeTime}");
                await Task.Delay(waitTime);
            }

            return gatewayShardingInfo;
        }

        private async Task StartShardsAsync(DiscordClientConfig config, GatewayShardingInfo gatewayShardingInfo)
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
                        tasks.Add(StartShardAsync(config, shardId));
                    }
                }

                DiscordClient.Logger.Log(LogLevel.Debug, $"Started {tasks.Count + _shards.Where(x => x is not null).Count()} out of {gatewayShardingInfo.Shards} shards");
                await Task.WhenAll(tasks);

                if (round < maxRounds - 1)
                {
                    DiscordClient.Logger.Log(LogLevel.Debug, $"Waiting 5 seconds till the next shards will be started");
                    await Task.Delay(5000);
                }
            }
        }

        private async Task StartShardAsync(DiscordClientConfig config, int shardId)
        {
            HandleDiscordPayload handleDiscordPayload = new(_discordClient, this);
            Shard shard = new(config, handleDiscordPayload, shardId);

            await shard.StartShardAsync();
            _shards[shardId] = shard;
        }

        public void ShardReady(ShardReadyEventArgs shardReadyEventArgs)
        {
            _shardsReady--;
            foreach (UnavailableGuild discordGuild in shardReadyEventArgs.Guilds)
            {
                _discordClient.InternalGuilds.TryAdd(discordGuild.Id, new(discordGuild));
            }
            
            if (_shardsReady == 0)
            {
                _discordClient.ShardsReady(shardReadyEventArgs);
                _shardsReady = null!;
            }
        }

        #endregion

        #region WssRequests

        public async Task RequestGuildMembersAsync(RequestGuildMembersCache requestGuildMembersCache)
        {
            int shardId = GetResponsibleShardId(requestGuildMembersCache.RequestGuildMembers.GuildId);
            await _shards[shardId].SendGuildMemberRequestAsync(requestGuildMembersCache);
        }

        public async Task<Dictionary<ulong, SoundboardSound[]>> RequestSoundboardSoundsAsync(ulong[] guildIds)
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

        public async Task SendShardSpecificMessageAsync<T>(ulong guildId, Payload<T> payload)
        {
            int shardID = GetResponsibleShardId(guildId);
            await _shards[shardID].SendPayloadWssAsync(payload);
        }

        public async Task SendGlobalWebSocketMessageAsync<T>(Payload<T> payload)
        {
            foreach (Shard shard in _shards)
            {
                await shard.SendPayloadWssAsync(payload);
            }
        }

        private int GetResponsibleShardId(ulong guildId)
            => (int)((guildId >> 22) % (ulong)TotalShards);
    }
}