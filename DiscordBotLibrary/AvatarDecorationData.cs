using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    public readonly struct AvatarDecorationData
    {
        [JsonPropertyName("asset")]
        public string Asset { get; init; }

        [JsonPropertyName("sku_id")]
        public string? SkuId { get; init; }
    }
}
