using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    public readonly struct WelcomeScreenChannel
    {
        [JsonPropertyName("channel_id")]
        public string ChannelId { get; init; }

        public string Description { get; init; }

        [JsonPropertyName("emoji_id")]
        public string? EmojiId { get; init; }

        [JsonPropertyName("emoji_name")]
        public string? EmojiName { get; init; }
    }
}