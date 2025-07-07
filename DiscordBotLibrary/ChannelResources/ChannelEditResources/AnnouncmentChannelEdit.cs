using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public sealed class AnnouncmentChannelEdit : BaseChannelEdit
    {
        /// <summary>
        /// <c> CAN ONLY BE <see cref="ChannelType.Announcement"/></c>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ChannelType> Type { get; set; }

        /// <summary>
        /// 0-1024 character
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<string> Topic { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Nsfw { get; set; }

        /// <summary>
        /// Adds/removes(set to null) this channel to a category
        /// </summary>
        [JsonProperty("parent_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ulong?> ParentCategory { get; set; }

        /// <summary>
        /// Default auto-archive duration (in minutes) for newly created threads.
        /// <para>Values: 60, 1440, 4320, 10080</para>
        /// </summary>
        [JsonProperty("default_auto_archive_duration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<AutoArchiveDuration> AutoArchiveDuration { get; set; }

        internal AnnouncmentChannelEdit(string name)
        {
            Name = name;
        }

        private AnnouncmentChannelEdit() { }
    }
}
