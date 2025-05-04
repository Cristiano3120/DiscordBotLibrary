using System.Text.Json.Serialization;
using DiscordBotLibrary.ChannelResources;

namespace DiscordBotLibrary.GuildCreateEvent
{
    internal class UnavailableGuildCreateEventArgs : IGuildCreateEventArgs
    {
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("joined_at")]
        public DateTimeOffset JoinedAt { get; init; }
        public bool Large { get; init; }

        public bool? Unavailable { get; init; }

        [JsonPropertyName("member_count")]
        public int MemberCount { get; init; }

        [JsonPropertyName("voice_states")]
        public List<VoiceState> VoiceStates { get; init; } = [];
        public List<Member> Members { get; init; } = [];
        public List<Channel> Channels { get; init; } = [];
        public List<Channel> Threads { get; init; } = [];
        public List<Presence> Presences { get; init; } = [];
    }
}
