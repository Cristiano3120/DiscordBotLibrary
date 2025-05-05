namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the status of a user across different platforms.
    /// </summary>
    public readonly struct ClientStatus
    {
        /// <summary>
        /// User's status set for an active desktop (Windows, Linux, Mac) application session
        /// </summary>
        [JsonPropertyName("desktop")]
        public string? Desktop { get; init; }

        /// <summary>
        /// User's status set for an active mobile (iOS, Android) application session
        /// </summary>
        [JsonPropertyName("mobile")]
        public string? Mobile { get; init; }

        /// <summary>
        /// User's status set for an active web (browser, bot user) application session
        /// </summary>
        [JsonPropertyName("web")]
        public string? Web { get; init; }
    }
}