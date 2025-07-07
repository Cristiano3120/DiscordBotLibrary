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
        [JsonProperty("emoji_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? EmojiId { get; init; }

        /// <summary>
        /// The unicode character of the emoji
        /// </summary>
        [JsonProperty("emoji_name")]
        public string? EmojiName { get; init; }
    }
}