namespace DiscordBotLibrary.EmbedResources
{
    public record EmbedField
    {
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        [JsonProperty("value")]
        public string Value { get; init; } = string.Empty;

        [JsonProperty("inline")]
        public bool? Inline { get; init; }
    }
}