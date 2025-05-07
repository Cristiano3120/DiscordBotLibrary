namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents a user's activity (e.g., playing a game, listening to music) in their Discord presence.
    /// </summary>
    public sealed record Activity
    {
        /// <summary>
        /// Gets the name of the activity.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Gets the type of activity.
        /// </summary>
        [JsonPropertyName("type")]
        public ActivityType Type { get; init; }

        /// <summary>
        /// Gets the stream URL, validated when the type is 1 (streaming).
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; init; }

        /// <summary>
        /// Gets the Unix timestamp (in milliseconds) when the activity was added to the user's session.
        /// </summary>
        [JsonPropertyName("created_at")]
        public long CreatedAt { get; init; }

        /// <summary>
        /// Gets the timestamps for the start and/or end of the activity.
        /// </summary>
        [JsonPropertyName("timestamps")]
        public ActivityTimestamps? Timestamps { get; init; }

        /// <summary>
        /// Gets the application ID for the game or activity.
        /// </summary>
        [JsonPropertyName("application_id")]
        public string? ApplicationId { get; init; }

        /// <summary>
        /// Gets the details of what the user is doing.
        /// </summary>
        [JsonPropertyName("details")]
        public string? Details { get; init; }

        /// <summary>
        /// Gets the user's current party status or custom status text.
        /// </summary>
        [JsonPropertyName("state")]
        public string? State { get; init; }

        /// <summary>
        /// Gets the emoji used for a custom status.
        /// </summary>
        [JsonPropertyName("emoji")]
        public ActivityEmoji? Emoji { get; init; }

        /// <summary>
        /// Gets information about the current party.
        /// </summary>
        [JsonPropertyName("party")]
        public ActivityParty? Party { get; init; }

        /// <summary>
        /// Gets the assets for the activity, such as images and hover text.
        /// </summary>
        [JsonPropertyName("assets")]
        public ActivityAssets? Assets { get; init; }

        /// <summary>
        /// Gets the secrets for Rich Presence joining and spectating.
        /// </summary>
        [JsonPropertyName("secrets")]
        public ActivitySecrets? Secrets { get; init; }

        /// <summary>
        /// Gets whether the activity is an instanced game session.
        /// </summary>
        [JsonPropertyName("instance")]
        public bool? Instance { get; init; }

        /// <summary>
        /// Gets the OR'd flags describing what the payload includes.
        /// </summary>
        [JsonPropertyName("flags")]
        public ActivityFlags Flags { get; init; }

        /// <summary>
        /// Gets custom buttons shown in the Rich Presence (maximum 2).
        /// </summary>
        [JsonPropertyName("buttons")]
        public ActivityButton[]? Buttons { get; init; }
    }
}
