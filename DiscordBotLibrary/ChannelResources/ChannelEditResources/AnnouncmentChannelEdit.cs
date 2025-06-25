namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public sealed class AnnouncmentChannelEdit : BaseChannelEdit
    {
        /// <summary>
        /// <c> CAN ONLY BE <see cref="ChannelType.Announcement"/></c>
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<ChannelType> Type { get; set; }

        /// <summary>
        /// 0-1024 character
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<string> Topic { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<bool> Nsfw { get; set; }

        /// <summary>
        /// Adds/removes(set to null) this channel to a category
        /// </summary>
        [JsonPropertyName("parent_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<ulong?> ParentCategory { get; set; }

        /// <summary>
        /// Default auto-archive duration (in minutes) for newly created threads.
        /// <para>Values: 60, 1440, 4320, 10080</para>
        /// </summary>
        [JsonPropertyName("default_auto_archive_duration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<int> AutoArchiveDuration { get; set; }

        internal AnnouncmentChannelEdit(string name)
        {
            Name = name;
        }

        private AnnouncmentChannelEdit() { }
    }
}
