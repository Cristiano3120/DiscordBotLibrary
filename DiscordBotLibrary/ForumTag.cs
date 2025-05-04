using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    public record ForumTag
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; init; } = default!;

        [JsonPropertyName("moderated")]
        public bool Moderated { get; init; }

        [JsonPropertyName("emoji_id")]
        public string? EmojiId { get; init; }

        [JsonPropertyName("emoji_name")]
        public string? EmojiName { get; init; }
    }
}