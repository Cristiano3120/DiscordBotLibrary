namespace DiscordBotLibrary.VoiceChannelHandling
{
    /// <summary>
    /// Represents a voice state in a Discord server.
    /// </summary>
    public record VoiceState
    {
        /// <summary>
        /// The guild ID this voice state is for (optional).  
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; init; }

        /// <summary>
        /// The channel ID this user is connected to (nullable).  
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("channel_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ChannelId { get; init; }

        /// <summary>
        /// The user ID this voice state is for.  
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("user_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong UserId { get; init; }

        /// <summary>
        /// The guild member this voice state is for (optional).
        /// </summary>
        [JsonPropertyName("member")]
        public GuildMember? Member { get; init; }

        /// <summary>
        /// The session ID for this voice state.
        /// </summary>
        [JsonPropertyName("session_id")]
        public string SessionId { get; init; } = string.Empty;

        /// <summary>
        /// Whether this user is deafened by the server.
        /// </summary>
        [JsonPropertyName("deaf")]
        public bool ServerDeaf { get; init; }

        /// <summary>
        /// Whether this user is muted by the server.
        /// </summary>
        [JsonPropertyName("mute")]
        public bool ServerMute { get; init; }

        /// <summary>
        /// Whether this user is locally deafened.
        /// </summary>
        [JsonPropertyName("self_deaf")]
        public bool SelfDeaf { get; init; }

        /// <summary>
        /// Whether this user is locally muted.
        /// </summary>
        [JsonPropertyName("self_mute")]
        public bool SelfMute { get; init; }

        /// <summary>
        /// Whether this user is streaming using "Go Live" (optional).
        /// </summary>
        [JsonPropertyName("self_stream")]
        public bool? SelfStream { get; init; }

        /// <summary>
        /// Whether this user's camera is enabled.
        /// </summary>
        [JsonPropertyName("self_video")]
        public bool SelfVideo { get; init; }

        /// <summary>
        /// Whether this user's permission to speak is denied.
        /// </summary>
        [JsonPropertyName("suppress")]
        public bool Suppress { get; init; }

        /// <summary>
        /// The time at which the user requested to speak (nullable ISO8601 timestamp).
        /// </summary>
        [JsonPropertyName("request_to_speak_timestamp")]
        public DateTimeOffset? RequestToSpeakTimestamp { get; init; }
    }
}
