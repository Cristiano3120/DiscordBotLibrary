using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents metadata associated with a Discord thread.
    /// </summary>
    public readonly struct ThreadMetadata
    {
        /// <summary>
        /// Whether the thread is archived.
        /// </summary>
        [JsonProperty("archived")]
        public bool Archived { get; init; }

        /// <summary>
        /// The thread will stop showing in the channel list after this many minutes of inactivity.
        /// Can be 60, 1440, 4320, or 10080.
        /// </summary>
        [JsonProperty("auto_archive_duration")]
        public AutoArchiveDuration AutoArchiveDuration { get; init; }

        /// <summary>
        /// ISO8601 timestamp of when the thread's archive status was last changed.
        /// Used for calculating recent activity.
        /// </summary>
        [JsonProperty("archive_timestamp")]
        public DateTime ArchiveTimestamp { get; init; }

        /// <summary>
        /// Whether the thread is locked.
        /// When a thread is locked, only users with the MANAGE_THREADS permission can unarchive it.
        /// </summary>
        [JsonProperty("locked")]
        public bool Locked { get; init; }

        /// <summary>
        /// Whether non-moderators can add other non-moderators to a thread.
        /// Only available on private threads.
        /// </summary>
        [JsonProperty("invitable")]
        public bool? Invitable { get; init; }

        /// <summary>
        /// ISO8601 timestamp of when the thread was created.
        /// Only populated for threads created after 2022-01-09.
        /// </summary>
        [JsonProperty("create_timestamp")]
        public DateTime? CreateTimestamp { get; init; }
    }

}
