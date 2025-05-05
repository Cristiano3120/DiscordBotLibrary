namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a default reaction for a thread.
    /// </summary>
    public readonly struct DefaultReaction
    {
        /// <summary>
        /// The id of a guild's custom emoji
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("emoji_id")]
        public string? EmojiId { get; init; }

        /// <summary>
        /// The unicode character of the emoji
        /// </summary>
        [JsonPropertyName("emoji_name")]
        public string? EmojiName { get; init; }
    }
}