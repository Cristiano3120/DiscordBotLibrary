using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.ChannelResources
{
    public record ChannelPinsUpdate
    {
        [JsonProperty("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; init; }

        [JsonProperty("channel_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong ChannelId { get; init; }

        [JsonProperty("last_pin_timestamp")]
        public DateTimeOffset? LastPinTimestamp { get; init; }
    }
}
