using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.VoiceChannelHandling
{
    internal readonly struct UpdateVoiceState
    {
        [JsonProperty("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong GuildId { get; init; }

        [JsonProperty("channel_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ChannelId { get; init; }

        [JsonProperty("self_deaf")]
        public bool SelfDeaf { get; init; }

        [JsonProperty("self_mute")]
        public bool SelfMute { get; init; }
    }
}
