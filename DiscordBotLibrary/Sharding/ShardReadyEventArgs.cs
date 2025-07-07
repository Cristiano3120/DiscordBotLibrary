namespace DiscordBotLibrary.Sharding
{
    /// <summary>
    /// Represents the payload received when the client successfully connects to the Discord Gateway.
    /// </summary>
    public sealed record ShardReadyEventArgs
    {
        /// <summary>
        /// API version.
        /// </summary>
        [JsonProperty("v")]
        public int Version { get; init; }

        /// <summary>
        /// Information about the connected user including email.
        /// </summary>
        [JsonProperty("user")]
        public User? DiscordUser { get; init; }

        /// <summary>
        /// Guilds the user is in (sent as UnavailableGuild objects).
        /// </summary>
        [JsonProperty("guilds")]
        public UnavailableGuild[] Guilds { get; init; } = [];

        /// <summary>
        /// Used for resuming connections.
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; init; } = string.Empty;

        /// <summary>
        /// Gateway URL for resuming connections.
        /// </summary>
        [JsonProperty("resume_gateway_url")]
        public string ResumeGatewayUri { get; init; } = string.Empty;

        /// <summary>
        /// Shard information associated with this session, if applicable.
        /// Format: [shard_id, num_shards]
        /// </summary>
        [JsonProperty("shard")]
        public int[]? Shard { get; init; }

        /// <summary>
        /// Contains infos about the application
        /// </summary>
        [JsonProperty("application")]
        public Application? Application { get; init; }
    }

}
