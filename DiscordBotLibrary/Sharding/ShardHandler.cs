using System.Text.Json;

namespace DiscordBotLibrary.Sharding
{
    internal static class ShardHandler
    {
        public static int TotalShards;
        private static Shard[] _shards = [];

        public static void Start(HttpClient httpClient)
        {
            _ = GetShardingInfos(httpClient);
        }

        private static async Task GetShardingInfos(HttpClient httpClient)
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

                await Task.WhenAll(tasks);

                DiscordClient.Logger.LogDebug($"Started {shards[round].Count} out of {gatewayShardingInfo.Shards} shards");
                if (round < maxRounds - 1)
                {
                    DiscordClient.Logger.LogDebug($"Waiting 5 seconds till the next {shards[++round].Count} shards will be started");
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

        private static int GetResponsibleShardId(ulong guildId, int totalShards)
            => (int)((guildId >> 22) % (ulong)totalShards);
    }
}