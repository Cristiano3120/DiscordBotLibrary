namespace DiscordBotLibrary.VoiceChannelHandling
{
    internal readonly struct UpdateVoiceState
    {
        [JsonPropertyName("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong GuildId { get; init; }

        [JsonPropertyName("channel_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ChannelId { get; init; }

        [JsonPropertyName("self_deaf")]
        public bool SelfDeaf { get; init; }

        [JsonPropertyName("self_mute")]
        public bool SelfMute { get; init; }
    }
}
