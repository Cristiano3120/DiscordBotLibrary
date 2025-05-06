namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a decoration data object for an avatar in Discord.
    /// </summary>
    public readonly struct AvatarDecorationData
    {
        [JsonPropertyName("asset")]
        public string Asset { get; init; }

        [JsonPropertyName("sku_id")]
        public string? SkuId { get; init; }
    }
}
