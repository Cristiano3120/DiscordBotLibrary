namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a decoration data object for an avatar in Discord.
    /// </summary>
    public readonly struct AvatarDecorationData
    {
        [JsonProperty("asset")]
        public string Asset { get; init; }

        [JsonProperty("sku_id")]
        public string? SkuId { get; init; }
    }
}
