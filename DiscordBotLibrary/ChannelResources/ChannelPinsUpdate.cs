namespace DiscordBotLibrary.ChannelResources
{
    public record ChannelPinsUpdate
    {
        [JsonPropertyName("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; init; }

        [JsonPropertyName("channel_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong ChannelId { get; init; }

        [JsonPropertyName("last_pin_timestamp")]
        public DateTimeOffset? LastPinTimestamp { get; init; }
    }
}
