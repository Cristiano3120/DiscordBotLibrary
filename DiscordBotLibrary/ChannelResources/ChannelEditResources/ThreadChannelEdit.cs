namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public class ThreadChannelEdit
    {
        public string Name { get; set; }

        /// <summary>
        /// whether the thread is archived
        /// </summary>
        [JsonPropertyName("archived")]
        public Optional<bool> Archived { get; set; }

        /// <summary>
        /// the thread will stop showing in the channel list after auto_archive_duration minutes of inactivity, 
        /// <para>can be set to: 60, 1440, 4320, 10080</para>
        /// </summary>
        [JsonPropertyName("auto_archive_duration")]
        public int AutoArchiveDuration { get; set; }

        /// <summary>
        /// whether the thread is locked; when a thread is locked, only users with MANAGE_THREADS can unarchive it
        /// </summary>
        [JsonPropertyName("locked")]
        public Optional<bool> Locked { get; set; }

        /// <summary>
        /// whether non-moderators can add other non-moderators to a thread; only available on private threads
        /// </summary>
        [JsonPropertyName("invitable")]
        public Optional<bool> Invitable { get; set; }

        /// <summary>
        /// Slowmode: How many seconds users must wait before sending another message.
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("rate_limit_per_user")]
        public Optional<int> Slowmode { get; set; }

        /// <summary>
        /// channel flags combined as a bitfield; PINNED can only be set for threads in forum and media channels
        /// </summary>
        [JsonPropertyName("flags")]
        public Optional<ChannelFlags> Flags { get; set; }

        /// <summary>
        /// the IDs of the set of tags that have been applied to a thread in a GUILD_FORUM or a GUILD_MEDIA channel; limited to 5
        /// </summary>
        [JsonPropertyName("applied_tags")]
        [JsonConverter(typeof(SnowflakeArrayConverter))]
        public Optional<ulong[]> AppliedTags { get; set; }

        internal ThreadChannelEdit(string name)
        {
            Name = name;
        }

        private ThreadChannelEdit() { Name = ""; }
    }
}
