namespace DiscordBotLibrary.EmbedResources
{
    /// <summary>
    /// Represents a Discord Embed object used in messages.
    /// </summary>
    public record Embed
    {
        /// <summary>
        /// Title of the embed.
        /// </summary>
        public string? Title { get; init; }

        /// <summary>
        /// Type of the embed (always "rich" for webhook embeds).
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmbedType? Type { get; init; }

        /// <summary>
        /// Description of the embed.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// URL of the embed.
        /// </summary>
        public string? Url { get; init; }

        /// <summary>
        /// ISO8601 timestamp of embed content.
        /// </summary>
        public DateTimeOffset? Timestamp { get; init; }

        /// <summary>
        /// Color code of the embed (decimal integer).
        /// </summary>
        public int? Color { get; init; }

        /// <summary>
        /// Footer information.
        /// </summary>
        public EmbedFooter? Footer { get; init; }

        /// <summary>
        /// Image information.
        /// </summary>
        public EmbedImage? Image { get; init; }

        /// <summary>
        /// Thumbnail information.
        /// </summary>
        public EmbedThumbnail? Thumbnail { get; init; }

        /// <summary>
        /// Video information.
        /// </summary>
        public EmbedVideo? Video { get; init; }

        /// <summary>
        /// Provider information.
        /// </summary>
        public EmbedProvider? Provider { get; init; }

        /// <summary>
        /// Author information.
        /// </summary>
        public EmbedAuthor? Author { get; init; }

        /// <summary>
        /// Fields information (max of 25).
        /// </summary>
        public EmbedField[]? Fields { get; init; }
    }
}