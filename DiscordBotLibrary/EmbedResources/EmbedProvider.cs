namespace DiscordBotLibrary.EmbedResources
{
    public record EmbedProvider
    {
        [JsonProperty("name")]
        public string? Name { get; init; }

        [JsonProperty("url")]
        public string? Url { get; init; }
    }
}