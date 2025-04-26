using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    internal record Sticker
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("format_type")]
        public int FormatType { get; init; }
        public string? Description { get; init; }
        public string Tags { get; init; } = string.Empty;
        public int Type { get; init; }
        public bool Available { get; init; }

        [JsonPropertyName("guild_id")]
        public string? GuildId { get; init; }

        [JsonPropertyName("user")]
        public DiscordUser? User { get; init; }

        [JsonPropertyName("sort_value")]
        public int? SortValue { get; init; }
    }
}