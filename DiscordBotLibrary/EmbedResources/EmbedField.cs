namespace DiscordBotLibrary.EmbedResources
{
    public record EmbedField
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("value")]
        public string Value { get; init; } = string.Empty;

        [JsonPropertyName("inline")]
        public bool? Inline { get; init; }
    }
}