namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a channel in the welcome screen.
    /// </summary>
    public readonly struct WelcomeScreenChannel
    {
        [JsonProperty("channel_id")]
        public string ChannelId { get; init; }

        [JsonProperty("description")]
        public string Description { get; init; }

        [JsonProperty("emoji_id")]
        public string? EmojiId { get; init; }

        [JsonProperty("emoji_name")]
        public string? EmojiName { get; init; }
    }
}