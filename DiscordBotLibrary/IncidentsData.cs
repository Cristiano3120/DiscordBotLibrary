namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents incidents-related data for a guild.
    /// </summary>
    public readonly struct IncidentsData
    {
        /// <summary>
        /// When invites get enabled again.
        /// </summary>
        [JsonPropertyName("invites_disabled_until")]
        public DateTimeOffset? InvitesDisabledUntil { get; init; }

        /// <summary>
        /// When direct messages get enabled again.
        /// </summary>
        [JsonPropertyName("dms_disabled_until")]
        public DateTimeOffset? DmsDisabledUntil { get; init; }

        /// <summary>
        /// When the DM spam was detected.
        /// </summary>
        [JsonPropertyName("dm_spam_detected_at")]
        public DateTimeOffset? DmSpamDetectedAt { get; init; }

        /// <summary>
        /// When the raid was detected.
        /// </summary>
        [JsonPropertyName("raid_detected_at")]
        public DateTimeOffset? RaidDetectedAt { get; init; }
    }
}