namespace DiscordBotLibrary.EmbedResources
{
    public record EmbedProvider
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("url")]
        public string? Url { get; init; }
    }
}