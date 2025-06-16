namespace DiscordBotLibrary.EmbedResources
{
    public record EmbedFooter
    {
        /// <summary>
        /// Gets or sets the text of the footer.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL of the footer icon.
        /// </summary>
        [JsonPropertyName("icon_url")]
        public string? IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the proxy URL for the footer icon.
        /// </summary>
        [JsonPropertyName("proxy_icon_url")]
        public string? ProxyIconUrl { get; set; }
    }
}