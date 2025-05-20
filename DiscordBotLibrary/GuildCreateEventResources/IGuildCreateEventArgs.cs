namespace DiscordBotLibrary.GuildCreateEventResources
{
    /// <summary>
    /// Represents the extra data provided when a guild is created or becomes available. 
    /// <para>
    /// /// You have to convert this object to the correct type.
    /// The correct type is determined by the <see cref="Unavailable"/> property 
    /// and can either be <see cref="GuildCreateEventArgs"/> or <see cref="UnavailableGuildCreateEventArgs"/>.
    /// </para>
    /// 
    /// <para>
    /// This object is probably an <see cref="GuildCreateEventArgs"/> object 
    /// as an <see cref="UnavailableGuildCreateEventArgs"/> obj only gets sent when the Guild has an outage.
    /// </para>
    /// </summary>
    public interface IGuildCreateEventArgs
    {
        /// <summary>
        /// Gets the date and time when this guild was joined.
        /// </summary>
        [JsonPropertyName("joined_at")]
        DateTime JoinedAt { get; init; }

        /// <summary>
        /// Gets a value indicating whether this is considered a large guild.
        /// </summary>
        [JsonPropertyName("large")]
        bool Large { get; init; }

        /// <summary>
        /// Gets a value indicating whether this guild is unavailable due to an outage.
        /// </summary>
        [JsonPropertyName("unavailable")]
        bool? Unavailable { get; init; }

        /// <summary>
        /// Gets the total number of members in this guild.
        /// </summary>
        [JsonPropertyName("member_count")]
        int MemberCount { get; init; }

        /// <summary>
        /// Gets the voice states of members currently in voice channels.
        /// These states do not include the guild_id key.
        /// </summary>
        [JsonPropertyName("voice_states")]
        VoiceState[] VoiceStates { get; init; }

        /// <summary>
        /// Gets the users currently in the guild.
        /// </summary>
        [JsonPropertyName("members")]
        GuildMember[] Members { get; init; }

        /// <summary>
        /// Gets the channels in the guild.
        /// </summary>
        [JsonPropertyName("channels")]
        Channel[] Channels { get; init; }

        /// <summary>
        /// Gets all active threads in the guild that the current user has permission to view.
        /// </summary>
        [JsonPropertyName("threads")]
        Channel[] Threads { get; init; }

        /// <summary>
        /// Gets the presence updates of the members in the guild.
        /// Will only include non-offline members if the size is greater than the large threshold.
        /// </summary>
        [JsonPropertyName("presences")]
        List<PresenceUpdate> Presences { get; init; }

        /// <summary>
        /// Gets the stage instances in the guild.
        /// </summary>
        [JsonPropertyName("stage_instances")]
        StageInstance[] StageInstances { get; init; }

        /// <summary>
        /// Gets the scheduled events in the guild.
        /// </summary>
        [JsonPropertyName("guild_scheduled_events")]
        GuildScheduledEvent[] GuildScheduledEvents { get; init; }

        /// <summary>
        /// Gets the soundboard sounds in the guild.
        /// </summary>
        [JsonPropertyName("soundboard_sounds")]
        SoundboardSound[] SoundboardSounds { get; init; }

        /// <summary>
        /// Trys to convert the <see cref="IGuildCreateEventArgs"/> to an <see cref="GuildCreateEventArgs"/> object.
        /// </summary>
        /// <returns>
        /// <para><c>If</c> convertable (<see cref="Unavailable"/> == <c>false</c>) an obj of type <see cref="GuildCreateEventArgs"/></para>
        /// <c>else</c> <c>null</c>.
        /// </returns>
        GuildCreateEventArgs? TryGetAvailableGuild();

        /// <summary>
        /// Trys to convert the <see cref="IGuildCreateEventArgs"/> to an <see cref="GuildCreateEventArgs"/> object.
        /// </summary>
        /// <returns>
        /// <para><c>If</c> convertable (<see cref="Unavailable"/> == <c>true</c>) an obj of type <see cref="GuildCreateEventArgs"/></para>
        /// <c>else</c> <c>null</c>.
        /// </returns>
        UnavailableGuildCreateEventArgs? TryGetUnavailableGuild();
    }
}
