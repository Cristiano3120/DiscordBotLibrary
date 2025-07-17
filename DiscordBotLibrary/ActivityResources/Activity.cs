using DiscordBotLibrary.Json.Converters;
using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

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
        [JsonProperty("application_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ApplicationId { get; init; }

        /// <summary>
        /// Name of the application – read-only field.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Type of activity (e.g., 0 = Game, 1 = Streaming).
        /// </summary>
        [JsonProperty("type")]
        public ActivityType Type { get; init; }

        /// <summary>
        /// The player's current party status or custom status text.
        /// </summary>
        [JsonProperty("state")]
        public string? State { get; init; }

        /// <summary>
        /// What the player is currently doing.
        /// </summary>
        [JsonProperty("details")]
        public string? Details { get; init; }

        /// <summary>
        /// Stream URL – validated when type is 1.
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; init; }

        /// <summary>
        /// Timestamp when the activity was added to the user's session.
        /// </summary>
        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? CreatedAt { get; init; }

        /// <summary>
        /// Helps create elapsed/remaining timestamps on a player's profile.
        /// </summary>
        [JsonProperty("timestamps")]
        public ActivityTimestamps? Timestamps { get; init; }

        /// <summary>
        /// Assets to display on the player's profile.
        /// </summary>
        [JsonProperty("assets")]
        public ActivityAssets? Assets { get; init; }

        /// <summary>
        /// Information about the player's party.
        /// </summary>
        [JsonProperty("party")]
        public ActivityParty? Party { get; init; }

        /// <summary>
        /// Secret passwords for joining and spectating the player's game.
        /// </summary>
        [JsonProperty("secrets")]
        public ActivitySecrets? Secrets { get; init; }

        /// <summary>
        /// Whether this activity is an instanced context, like a match.
        /// </summary>
        [JsonProperty("instance")]
        public bool? Instance { get; init; }

        /// <summary>
        /// Activity flags OR'd together, describes what the payload includes.
        /// </summary>
        [JsonProperty("flags")]
        public ActivityFlags? Flags { get; init; }

        /// <summary>
        /// Emoji used for a custom status.
        /// </summary>
        [JsonProperty("emoji")]
        public ActivityEmoji? Emoji { get; init; }

        /// <summary>
        /// Custom buttons shown in the Rich Presence (max 2).
        /// </summary>
        [JsonProperty("buttons")]
        [JsonConverter(typeof(ActivityButtonConverter))]
        public ActivityButton[]? Buttons { get; init; }

        /// <summary>
        /// Internal session ID, used e.g. for Spotify sync.
        /// </summary>
        [JsonProperty("session_id")]
        public string? SessionId { get; init; }

        /// <summary>
        /// Sync ID used for services like Spotify.
        /// </summary>
        [JsonProperty("sync_id")]
        public string? SyncId { get; init; }

        /// <summary>
        /// Activity ID – usually custom like "spotify:1" or null.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; init; }
    }
}
