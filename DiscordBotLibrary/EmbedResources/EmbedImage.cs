namespace DiscordBotLibrary.EmbedResources
{
    public record EmbedImage
    {
        /// <summary>
        /// URL of the image.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; init; } = string.Empty;
        /// <summary>
        /// A proxied URL of the image.
        /// </summary>
        [JsonProperty("proxy_url")]
        public string? ProxyUrl { get; init; }
        /// <summary>
        /// Height of the image in pixels.
        /// </summary>
        [JsonProperty("height")]
        public int? Height { get; init; }
        /// <summary>
        /// Width of the image in pixels.
        /// </summary>
        [JsonProperty("width")]
        public int? Width { get; init; }
    }
}