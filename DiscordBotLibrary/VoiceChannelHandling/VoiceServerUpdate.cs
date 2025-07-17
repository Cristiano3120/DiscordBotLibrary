using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.VoiceChannelHandling
{
    internal sealed class VoiceServerUpdate
    {
        [JsonProperty("token")]
        public string Token  { get; init; } = string.Empty;

        [JsonConverter(typeof(SnowflakeConverter))]
        [JsonProperty("guild_id")]
        public ulong GuildId { get; init; }

        [JsonProperty("endpoint")]
        public string Endpoint { get; init; } = string.Empty;
    }
}
