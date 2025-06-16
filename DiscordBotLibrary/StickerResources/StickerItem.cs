namespace DiscordBotLibrary.StickerResources
{
    public record StickerItem
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong StickerId { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("format_type")]
        public StickerType FormatType { get; init; }
    }
}