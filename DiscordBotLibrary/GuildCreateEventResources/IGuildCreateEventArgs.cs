namespace DiscordBotLibrary.GuildCreateEventResources
{
    /// <summary>
    /// Represents the extra data provided when a guild is created or becomes available.
    /// </summary>
    internal interface IGuildCreateEventArgs
    {
        /// <summary>
        /// Gets the date and time when this guild was joined.
        /// </summary>
        [JsonPropertyName("joined_at")]
        public DateTime JoinedAt { get; init; }

        /// <summary>
        /// Gets a value indicating whether this is considered a large guild.
        /// </summary>
        [JsonPropertyName("large")]
        public bool Large { get; init; }

        /// <summary>
        /// Gets a value indicating whether this guild is unavailable due to an outage.
        /// </summary>
        [JsonPropertyName("unavailable")]
        public bool? Unavailable { get; init; }

        /// <summary>
        /// Gets the total number of members in this guild.
        /// </summary>
        [JsonPropertyName("member_count")]
        public int MemberCount { get; init; }

        /// <summary>
        /// Gets the voice states of members currently in voice channels.
        /// These states do not include the guild_id key.
        /// </summary>
        [JsonPropertyName("voice_states")]
        public VoiceState[] VoiceStates { get; init; }

        /// <summary>
        /// Gets the users currently in the guild.
        /// </summary>
        [JsonPropertyName("members")]
        public GuildMember[] Members { get; init; }

        /// <summary>
        /// Gets the channels in the guild.
        /// </summary>
        [JsonPropertyName("channels")]
        public Channel[] Channels { get; init; }

        /// <summary>
        /// Gets all active threads in the guild that the current user has permission to view.
        /// </summary>
        [JsonPropertyName("threads")]
        public Channel[] Threads { get; init; }

        /// <summary>
        /// Gets the presence updates of the members in the guild.
        /// Will only include non-offline members if the size is greater than the large threshold.
        /// </summary>
        [JsonPropertyName("presences")]
        public PresenceUpdate[] Presences { get; init; }

        /// <summary>
        /// Gets the stage instances in the guild.
        /// </summary>
        [JsonPropertyName("stage_instances")]
        public StageInstance[] StageInstances { get; init; }

        /// <summary>
        /// Gets the scheduled events in the guild.
        /// </summary>
        [JsonPropertyName("guild_scheduled_events")]
        public GuildScheduledEvent[] GuildScheduledEvents { get; init; }

        /// <summary>
        /// Gets the soundboard sounds in the guild.
        /// </summary>
        [JsonPropertyName("soundboard_sounds")]
        public SoundboardSound[] SoundboardSounds { get; init; }
    }
}
