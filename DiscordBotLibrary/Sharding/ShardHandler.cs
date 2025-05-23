﻿using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBotLibrary.Sharding
{
    internal static class ShardHandler
    {
        public static ReadyEventArgs ReadyEventArgs { get; set; } = new();
        public static int TotalShards { get; private set; }

        private static HashSet<int> _shardIds = new();
        private static Shard[] _shards = [];

        public static void Start(HttpClient httpClient)
            => _ = InitAndConnectShardsAsync(httpClient);
        
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

        public static void ShardReady(ShardReadyEventArgs shardReadyEventArgs)
        {
            _shardIds.Add(shardReadyEventArgs.Shard![0]);
            ReadyEventArgs.Guilds = [..ReadyEventArgs.Guilds, ..shardReadyEventArgs.Guilds];

            if (_shardIds.Count == TotalShards)
            {
                DiscordClient client = DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>();
                client.InvokeOnReady(ReadyEventArgs);

                _shardIds = null!;
            }
        }

        public static async Task SendGlobalWebSocketMessageAsync(object payload)
        {
            foreach (Shard shard in _shards)
            {
                await shard.SendPayloadWssAsync(payload);
            }
        }

        private static int GetResponsibleShardId(ulong guildId, int totalShards)
            => (int)((guildId >> 22) % (ulong)totalShards);
    }
}