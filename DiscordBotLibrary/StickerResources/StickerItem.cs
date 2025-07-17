using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.StickerResources
{
    public record StickerItem
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong StickerId { get; init; }

        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        [JsonProperty("format_type")]
        public StickerType FormatType { get; init; }
    }
}