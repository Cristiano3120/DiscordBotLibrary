namespace DiscordBotLibrary.GuildCreateEventResources
{
    /// <summary>
    /// Represents the extra data provided by the <see cref="Events.GUILD_CREATE"/> event. 
    /// This object represents a guild that is unavailable.
    /// </summary>
    internal class UnavailableGuildCreateEventArgs : IGuildCreateEventArgs
    {
        /// <summary>
        /// The id of the guild
        /// TYPE: SNOWFLAKE
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

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
        public GuildMember[] Members { get; init; } = [];

        [JsonPropertyName("channels")]
        public Channel[] Channels { get; init; } = [];

        [JsonPropertyName("threads")]
        public Channel[] Threads { get; init; } = [];

        [JsonPropertyName("presences")]
        public PresenceUpdate[] Presences { get; init; } = [];

        [JsonPropertyName("stage_instances")]
        public StageInstance[] StageInstances { get; init; } = [];

        [JsonPropertyName("guild_scheduled_events")]
        public GuildScheduledEvent[] GuildScheduledEvents { get; init; } = [];

        [JsonPropertyName("soundboard_sounds")]
        public SoundboardSound[] SoundboardSounds { get; init; } = [];
    }
}
