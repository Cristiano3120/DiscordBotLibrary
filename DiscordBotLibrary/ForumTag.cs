namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a forum tag in a Discord forum channel.
    /// </summary>
    public record ForumTag
    {
        /// <summary>
        /// The unique ID of the tag.
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; }

        /// <summary>
        /// The name of the tag (0-20 characters).
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// Whether the tag is moderated and requires MANAGE_THREADS permission to modify.
        /// </summary>
        [JsonPropertyName("moderated")]
        public bool Moderated { get; init; }

        /// <summary>
        /// The ID of a custom emoji associated with this tag
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("emoji_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? EmojiId { get; init; }

        /// <summary>
        /// The Unicode emoji character associated with this tag
        /// </summary>
        [JsonPropertyName("emoji_name")]
        public string? EmojiName { get; init; }
    }
}