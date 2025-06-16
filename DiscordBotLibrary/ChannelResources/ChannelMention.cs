namespace DiscordBotLibrary.ChannelResources
{
    public record ChannelMention
    {
        [JsonPropertyName("id")]
        public ulong Id { get; init; }

        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; init; }

        [JsonPropertyName("type")]
        public ChannelType Type { get; init; }

        /// <summary>
        /// Channel name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = default!;
    }
}