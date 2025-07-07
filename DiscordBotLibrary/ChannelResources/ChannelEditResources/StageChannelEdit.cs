using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public sealed class StageChannelEdit : BaseChannelEdit
    {
        /// <summary>
        /// whether the channel is nsfw
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Nsfw { get; set; }

        /// <summary>
        /// Slowmode: How many seconds users must wait before sending another message.
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
        /// </summary>
        [JsonProperty("rate_limit_per_user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<Slowmode> Slowmode { get; set; }

        /// <summary>
        /// Gets or sets the bitrate value, in kilobits per second (kbps), for the associated channel(8K-384K) depending on the server level.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<uint> Bitrate { get; set; }

        /// <summary>
        /// The user limit of the voice or stage channel
        /// <para>Max 99 for voice channels and 10,000 for stage channels (0 refers to no limit)</para>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<int> UserLimit { get; set; }

        [JsonProperty("parent_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ulong?> ParentCategory { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<RtcRegion?> RtcRegion
        {
            get;
            set
            {
                if (value.HasValue && value.Value == ChannelEnums.RtcRegion.Automatic)
                {
                    field = null;
                }
            }
        }

        /// <summary>
        /// the camera video quality mode of the voice channe
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<VideoQualityMode> VideoQualityMode { get; set; }

        internal StageChannelEdit(string name)
        {
            Name = name;
        }

        private StageChannelEdit() { }
    }
}
