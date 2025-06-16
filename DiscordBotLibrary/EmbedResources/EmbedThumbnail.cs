namespace DiscordBotLibrary.EmbedResources
{
    public record EmbedThumbnail
    {
        /// <summary>
        /// URL of the image.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; init; } = string.Empty;
        /// <summary>
        /// A proxied URL of the image.
        /// </summary>
        [JsonPropertyName("proxy_url")]
        public string? ProxyUrl { get; init; }
        /// <summary>
        /// Height of the image in pixels.
        /// </summary>
        [JsonPropertyName("height")]
        public int? Height { get; init; }
        /// <summary>
        /// Width of the image in pixels.
        /// </summary>
        [JsonPropertyName("width")]
        public int? Width { get; init; }
    }
}