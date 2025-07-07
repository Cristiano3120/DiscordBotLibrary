using System.Text.Json.Serialization;
using DiscordBotLibrary.GuildMemberResources;

namespace DiscordBotLibrary.ThreadMemberResources
{
    public readonly struct ThreadMember
    {
        [JsonProperty("id")]
        public string? ThreadId { get; init; }

        [JsonProperty("user_id")]
        public string? UserId { get; init; }

        [JsonProperty("join_timestamp")]
        public DateTime JoinTimestamp { get; init; }

        [JsonProperty("flags")]
        public ThreadMemberFlags Flags { get; init; }

        [JsonProperty("member")]
        public GuildMember? Member { get; init; }
    }
}