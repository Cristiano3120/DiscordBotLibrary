using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary.ChannelResources
{
    public record ChannelMention
    {
        [JsonProperty("id")]
        public ulong Id { get; init; }

        [JsonProperty("guild_id")]
        public ulong GuildId { get; init; }

        [JsonProperty("type")]
        public ChannelType Type { get; init; }

        /// <summary>
        /// Channel name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = default!;
    }
}