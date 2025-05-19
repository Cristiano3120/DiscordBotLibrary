namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents a user's activity (e.g., playing a game, listening to music) in their Discord presence.
    /// </summary>
    public sealed record Activity
    {
        /// <summary>
        /// TYPE: Snowflake
        /// Your application ID – read-only field.
        /// </summary>
        [JsonPropertyName("application_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ApplicationId { get; init; }

        /// <summary>
        /// Name of the application – read-only field.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Type of activity (e.g., 0 = Game, 1 = Streaming).
        /// </summary>
        [JsonPropertyName("type")]
        public ActivityType Type { get; init; }

        /// <summary>
        /// The player's current party status or custom status text.
        /// </summary>
        [JsonPropertyName("state")]
        public string? State { get; init; }

        /// <summary>
        /// What the player is currently doing.
        /// </summary>
        [JsonPropertyName("details")]
        public string? Details { get; init; }

        /// <summary>
        /// Stream URL – validated when type is 1.
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; init; }

        /// <summary>
        /// Timestamp when the activity was added to the user's session.
        /// </summary>
        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? CreatedAt { get; init; }

        /// <summary>
        /// Helps create elapsed/remaining timestamps on a player's profile.
        /// </summary>
        [JsonPropertyName("timestamps")]
        public ActivityTimestamps? Timestamps { get; init; }

        /// <summary>
        /// Assets to display on the player's profile.
        /// </summary>
        [JsonPropertyName("assets")]
        public ActivityAssets? Assets { get; init; }

        /// <summary>
        /// Information about the player's party.
        /// </summary>
        [JsonPropertyName("party")]
        public ActivityParty? Party { get; init; }

        /// <summary>
        /// Secret passwords for joining and spectating the player's game.
        /// </summary>
        [JsonPropertyName("secrets")]
        public ActivitySecrets? Secrets { get; init; }

        /// <summary>
        /// Whether this activity is an instanced context, like a match.
        /// </summary>
        [JsonPropertyName("instance")]
        public bool? Instance { get; init; }

        /// <summary>
        /// Activity flags OR'd together, describes what the payload includes.
        /// </summary>
        [JsonPropertyName("flags")]
        public ActivityFlags? Flags { get; init; }

        /// <summary>
        /// Emoji used for a custom status.
        /// </summary>
        [JsonPropertyName("emoji")]
        public ActivityEmoji? Emoji { get; init; }

        /// <summary>
        /// Custom buttons shown in the Rich Presence (max 2).
        /// </summary>
        [JsonPropertyName("buttons")]
        public ActivityButton[]? Buttons { get; init; }

        /// <summary>
        /// Internal session ID, used e.g. for Spotify sync.
        /// </summary>
        [JsonPropertyName("session_id")]
        public string? SessionId { get; init; }

        /// <summary>
        /// Sync ID used for services like Spotify.
        /// </summary>
        [JsonPropertyName("sync_id")]
        public string? SyncId { get; init; }

        /// <summary>
        /// Activity ID – usually custom like "spotify:1" or null.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; init; }
    }
}
