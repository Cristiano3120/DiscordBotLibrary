using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public sealed class TextChannelEdit : BaseChannelEdit
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
        /// Slowmode: How many seconds users must wait before sending another message.
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
        /// </summary>
        [JsonProperty("rate_limit_per_user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<Slowmode> Slowmode { get; set; }

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

        /// <summary>
        /// the initial rate_limit_per_user to set on newly created threads in a channel.
        /// <para>this field is copied to the thread at creation time and does not live update.</para>
        /// </summary>
        [JsonProperty("default_thread_rate_limit_per_user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<int> ThreadRateLimit { get; set; }

        internal TextChannelEdit(string name)
        {
            Name = name;
        }

        private TextChannelEdit() { }
    }
}
