namespace DiscordBotLibrary.EmbedResources
{
    /// <summary>
    /// Represents the author information of a Discord Embed.
    /// </summary>
    public record EmbedAuthor
    {
        /// <summary>
        /// Name of the author.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// URL of the author (only supports http(s)).
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; init; }

        /// <summary>
        /// URL of the author icon (only supports http(s) and attachments).
        /// </summary>
        [JsonProperty("icon_url")]
        public string? IconUrl { get; init; }

        /// <summary>
        /// A proxied URL of the author icon.
        /// </summary>
        [JsonProperty("proxy_icon_url")]
        public string? ProxyIconUrl { get; init; }
    }

}