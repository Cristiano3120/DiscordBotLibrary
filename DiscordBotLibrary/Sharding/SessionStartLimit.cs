namespace DiscordBotLibrary.Sharding
{
    /// <summary>
    /// Represents the limits and settings related to session starts in a given context.
    /// </summary>
    internal readonly struct SessionStartLimit
    {
        /// <summary>
        /// Total number of session starts the current user is allowed
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; init; }

        /// <summary>
        /// Remaining number of session starts the current user is allowed
        /// </summary>
        [JsonPropertyName("remaining")]
        public int Remaining { get; init; }

        /// <summary>
        /// Number of milliseconds after which the limit resets
        /// </summary>
        [JsonPropertyName("reset_after")]
        public int ResetAfter { get; init; }

        /// <summary>
        /// Number of identify requests allowed per 5 seconds
        /// </summary>
        [JsonPropertyName("max_concurrency")]
        public int MaxConcurrency { get; init; }
    }
}