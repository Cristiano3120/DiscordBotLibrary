using System.Text.Json.Serialization;
using DiscordBotLibrary.GuildMemberResources;

namespace DiscordBotLibrary.ThreadMemberResources
{
    public readonly struct ThreadMember
    {
        [JsonPropertyName("id")]
        public string? ThreadId { get; init; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; init; }

        [JsonPropertyName("join_timestamp")]
        public DateTime JoinTimestamp { get; init; }

        [JsonPropertyName("flags")]
        public ThreadMemberFlags Flags { get; init; }

        [JsonPropertyName("member")]
        public GuildMember? Member { get; init; }
    }
}