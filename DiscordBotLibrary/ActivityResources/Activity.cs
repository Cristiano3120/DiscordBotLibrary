namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents a user's activity (e.g., playing a game, listening to music) in their Discord presence.
    /// </summary>
    public record Activity
    {
        /// <summary>
        /// TYPE: Snowflake
        /// Your application ID – read-only field.
        /// </summary>
        [JsonPropertyName("application_id")]
        public ulong ApplicationId { get; init; }

        /// <summary>
        /// Name of the application – read-only field.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; }

        /// <summary>
        /// The player's current party status.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; init; }

        /// <summary>
        /// What the player is currently doing.
        /// </summary>
        [JsonPropertyName("details")]
        public string? Details { get; init; }

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
    }
}
