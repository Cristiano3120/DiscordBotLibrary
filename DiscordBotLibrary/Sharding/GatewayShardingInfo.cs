namespace DiscordBotLibrary.Sharding
{
    /// <summary>
    /// Represents information required for connecting to a Gateway, including the WebSocket URL,  recommended shard
    /// count, and session start limit details.
    /// </summary>
    internal readonly struct GatewayShardingInfo
    {
        /// <summary>
        /// WSS URL that can be used for connecting to the Gateway
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; init; }

        /// <summary>
        /// Recommended number of <see cref="Shard"/>s to use when connecting
        /// </summary>
        [JsonProperty("shards")]
        public int Shards { get; init; }

        /// <summary>
        /// Information on the current session start limit
        /// </summary>
        [JsonProperty("session_start_limit")]
        public SessionStartLimit SessionStartLimit { get; init; }
    }
}
