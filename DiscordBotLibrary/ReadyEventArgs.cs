using DiscordBotLibrary.ApplicationResources;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the payload received when the client successfully connects to the Discord Gateway.
    /// </summary>
    internal record ReadyEventArgs
    {
        /// <summary>
        /// API version.
        /// </summary>
        [JsonPropertyName("v")]
        public int Version { get; init; }

        /// <summary>
        /// Information about the connected user including email.
        /// </summary>
        [JsonPropertyName("user")]
        public User? DiscordUser { get; init; }

        /// <summary>
        /// Guilds the user is in (sent as UnavailableGuild objects).
        /// </summary>
        [JsonPropertyName("guilds")]
        public UnavailableGuild[] Guilds { get; init; } = [];

        /// <summary>
        /// Used for resuming connections.
        /// </summary>
        [JsonPropertyName("session_id")]
        public string SessionId { get; init; } = string.Empty;

        /// <summary>
        /// Gateway URL for resuming connections.
        /// </summary>
        [JsonPropertyName("resume_gateway_url")]
        public string ResumeGatewayUri { get; init; } = string.Empty;

        /// <summary>
        /// Shard information associated with this session, if applicable.
        /// Format: [shard_id, num_shards]
        /// </summary>
        [JsonPropertyName("shard")]
        public int[]? Shard { get; init; }

        /// <summary>
        /// Contains infos about the application
        /// </summary>
        [JsonPropertyName("application")]
        public Application? Application { get; init; }
    }

}
