using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public class ThreadChannelEdit
    {
        public string Name { get; set; }

        /// <summary>
        /// whether the thread is archived
        /// </summary>
        [JsonProperty("archived", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Archived { get; set; }

        /// <summary>
        /// the thread will stop showing in the channel list after auto_archive_duration minutes of inactivity, 
        /// <para>can be set to: 60, 1440, 4320, 10080</para>
        /// </summary>
        [JsonProperty("auto_archive_duration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<AutoArchiveDuration> AutoArchiveDuration { get; set; }

        /// <summary>
        /// whether the thread is locked; when a thread is locked, only users with MANAGE_THREADS can unarchive it
        /// </summary>
        [JsonProperty("locked", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Locked { get; set; }

        /// <summary>
        /// whether non-moderators can add other non-moderators to a thread; only available on private threads
        /// </summary>
        [JsonProperty("invitable", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Invitable { get; set; }

        /// <summary>
        /// Slowmode: How many seconds users must wait before sending another message.
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
        /// </summary>
        [JsonProperty("rate_limit_per_user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<Slowmode> Slowmode { get; set; }

        /// <summary>
        /// channel flags combined as a bitfield; PINNED can only be set for threads in forum and media channels
        /// </summary>
        [JsonProperty("flags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ChannelFlags> Flags { get; set; }

        /// <summary>
        /// the IDs of the set of tags that have been applied to a thread in a GUILD_FORUM or a GUILD_MEDIA channel; limited to 5
        /// </summary>
        [JsonProperty("applied_tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ulong[]> AppliedTags { get; set; }

        internal ThreadChannelEdit(string name)
        {
            Name = name;
        }

        private ThreadChannelEdit() { Name = ""; }
    }
}
