﻿namespace DiscordBotLibrary.GuildCreateEventResources
{
    /// <summary>
    /// Represents the extra data provided when a <see cref="Event.GUILD_CREATE"/> event is fired by the Discord API.
    /// </summary>
    public record GuildCreateEventArgs : Guild, IGuildCreateEventArgs
    {
        [JsonPropertyName("joined_at")]
        public DateTime JoinedAt { get; init; }

        [JsonPropertyName("large")]
        public bool Large { get; init; }

        [JsonPropertyName("unavailable")]
        public bool? Unavailable { get; init; }

        [JsonPropertyName("member_count")]
        public int MemberCount { get; init; }

        [JsonPropertyName("voice_states")]
        public VoiceState[] VoiceStates { get; init; } = [];

        [JsonPropertyName("members")]
        public List<GuildMember> Members { get; init; } = [];

        [JsonPropertyName("channels")]
        public Channel[] Channels { get; init; } = [];

        [JsonPropertyName("threads")]
        public Channel[] Threads { get; init; } = [];

        [JsonPropertyName("presences")]
        public List<Presence> Presences { get; init; } = [];

        [JsonPropertyName("stage_instances")]
        public StageInstance[] StageInstances { get; init; } = [];

        [JsonPropertyName("guild_scheduled_events")]
        public GuildScheduledEvent[] GuildScheduledEvents { get; init; } = [];

        [JsonPropertyName("soundboard_sounds")]
        public SoundboardSound[] SoundboardSounds { get; protected set; } = [];

        public GuildCreateEventArgs? TryGetAvailableGuild() => this;

        public UnavailableGuildCreateEventArgs? TryGetUnavailableGuild() => null;
    }
}
