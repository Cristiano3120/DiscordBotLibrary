using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    internal record Role
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Color { get; set; }
        public bool Hoist { get; set; }
        public string? Icon { get; set; }

        [JsonPropertyName("unicode_emoji")]
        public string? UnicodeEmoji { get; set; }
        public int Position { get; set; }
        public string Permissions { get; set; } = default!;
        public bool Managed { get; set; }
        public bool Mentionable { get; set; }
        public RoleTags? Tags { get; set; }
        public int Flags { get; set; }
    }
}
