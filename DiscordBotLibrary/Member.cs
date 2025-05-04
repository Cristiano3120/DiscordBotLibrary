using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    public class Member
    {
        public required User User { get; init; }

        [JsonPropertyName("nick")]
        public string? Nickname { get; init; }

        [JsonPropertyName("roles")]
        public List<string> Roles { get; init; } = [];

        [JsonPropertyName("joined_at")]
        public DateTimeOffset JoinedAt { get; init; }

        [JsonPropertyName("premium_since")]
        public DateTimeOffset? PremiumSince { get; init; }
        public bool Deaf { get; init; }
        public bool Mute { get; init; }
        public bool Pending { get; init; }

        [JsonPropertyName("communication_disabled_until")]
        public DateTimeOffset? CommunicationDisabledUntil { get; init; }

        public string? Avatar { get; init; }
    }
}