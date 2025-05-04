namespace DiscordBotLibrary
{
    using System.Text.Json.Serialization;

    public record Role
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("color")]
        public int Color { get; set; }

        [JsonPropertyName("hoist")]
        public bool Hoist { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("unicode_emoji")]
        public string? UnicodeEmoji { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("permissions")]
        public string Permissions { get; set; } = default!;

        [JsonPropertyName("managed")]
        public bool Managed { get; set; }

        [JsonPropertyName("mentionable")]
        public bool Mentionable { get; set; }

        [JsonPropertyName("tags")]
        public RoleTags? Tags { get; set; }

        [JsonPropertyName("flags")]
        public int Flags { get; set; }
    }

}
