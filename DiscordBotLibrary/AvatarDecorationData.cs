using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    internal readonly struct AvatarDecorationData
    {
        public string Asset { get; init; }

        [JsonPropertyName("sku_id")]
        public string? SkuId { get; init; }
    }
}
