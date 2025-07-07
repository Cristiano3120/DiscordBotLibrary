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
        [JsonProperty("desktop")]
        public string? Desktop { get; init; }

        /// <summary>
        /// User's status set for an active mobile (iOS, Android) application session
        /// </summary>
        [JsonProperty("mobile")]
        public string? Mobile { get; init; }

        /// <summary>
        /// User's status set for an active web (browser, bot user) application session
        /// </summary>
        [JsonProperty("web")]
        public string? Web { get; init; }
    }
}