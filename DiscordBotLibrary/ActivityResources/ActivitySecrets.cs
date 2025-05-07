namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents the secrets for an activity.
    /// </summary>
    public readonly struct ActivitySecrets
    {
        /// <summary>
        /// unique hash for the given match context
        /// </summary>
        [JsonPropertyName("join")]
        public string? Join { get; init; }

        /// <summary>
        /// unique hash for chat invites and Ask to Join
        /// </summary>
        [JsonPropertyName("spectate")]
        public string? Spectate { get; init; }

        /// <summary>
        /// unique hash for Spectate button
        /// </summary>
        [JsonPropertyName("match")]
        public string? Match { get; init; }
    }
}