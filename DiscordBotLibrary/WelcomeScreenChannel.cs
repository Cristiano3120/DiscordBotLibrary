namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a channel in the welcome screen.
    /// </summary>
    public readonly struct WelcomeScreenChannel
    {
        [JsonPropertyName("channel_id")]
        public string ChannelId { get; init; }

        [JsonPropertyName("description")]
        public string Description { get; init; }

        [JsonPropertyName("emoji_id")]
        public string? EmojiId { get; init; }

        [JsonPropertyName("emoji_name")]
        public string? EmojiName { get; init; }
    }
}