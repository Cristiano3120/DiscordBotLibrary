namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the welcome screen of a server
    /// </summary>
    public readonly struct WelcomeScreen
    {
        /// <summary>
        /// The server description shown in the welcome screen
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; init; }


        /// <summary>
        /// The channels shown in the welcome screen, up to 5
        /// </summary>
        [JsonProperty("welcome_channels")]
        public WelcomeScreenChannel[] WelcomeChannels { get; init; }
    }
}
