using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    public readonly struct DefaultReaction
    {
        [JsonPropertyName("emoji_id")]
        public string? EmojiId { get; init; }

        [JsonPropertyName("emoji_name")]
        public string? EmojiName { get; init; }
    }
}