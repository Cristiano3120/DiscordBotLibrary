namespace DiscordBotLibrary.VoiceChannelHandling
{
    internal sealed class VoiceServerUpdate
    {
        [JsonPropertyName("token")]
        public string Token  { get; init; } = string.Empty;

        [JsonConverter(typeof(SnowflakeConverter))]
        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; init; }

        [JsonPropertyName("endpoint")]
        public string Endpoint { get; init; } = string.Empty;
    }
}
