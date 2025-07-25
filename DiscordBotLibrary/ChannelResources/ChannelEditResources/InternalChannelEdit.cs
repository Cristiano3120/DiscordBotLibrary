﻿using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    /// <summary>
    /// Contains all the properties that could be changed theoretically in a channel edit request.
    /// Only for internal use.
    /// </summary>
    internal sealed class InternalChannelEdit : BaseChannelEdit
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

        /// <summary>
        /// Slowmode: How many seconds users must wait before sending another message.
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
        /// </summary>
        [JsonProperty("rate_limit_per_user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<Slowmode> Slowmode { get; set; }

        /// <summary>
        ///  Currently only REQUIRE_TAG (1 << 4) is supported for this type of channel
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ChannelFlags> Flags { get; set; }

        /// <summary>
        /// the set of tags that can be used in a GUILD_FORUM or a GUILD_MEDIA channel;
        /// <para>limited to 20</para>
        /// </summary>
        [JsonProperty("available_tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ForumTag[]> Tag { get; set; }

        /// <summary>
        /// the emoji to show in the add reaction button on a thread in a GUILD_FORUM or a GUILD_MEDIA channel
        /// </summary>
        [JsonProperty("default_reaction_emoji", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<DefaultReaction> Reaction { get; set; }

        /// <summary>
        /// the initial rate_limit_per_user to set on newly created threads in a channel.
        /// <para>this field is copied to the thread at creation time and does not live update.</para>
        /// </summary>
        [JsonProperty("default_thread_rate_limit_per_user", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<uint> ThreadRateLimit { get; set; }

        /// <summary>
        /// the default sort order type used to order posts in GUILD_FORUM and GUILD_MEDIA channels
        /// </summary>
        [JsonProperty("default_sort_order", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<SortOrderType?> SortOrder { get; set; }

        [JsonProperty("default_forum_layout", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ForumLayoutType> LayoutType { get; set; }

        /// <summary>
        /// icon: binary as base64 encoded 
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<string> Icon { get; set; }

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
        public Optional<uint> UserLimit { get; set; }

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

        /// <summary>
        /// whether the thread is archived
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Archived { get; set; }

        /// <summary>
        /// whether the thread is locked; when a thread is locked, only users with MANAGE_THREADS can unarchive it
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Locked { get; set; }

        /// <summary>
        /// whether non-moderators can add other non-moderators to a thread; only available on private threads
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Invitable { get; set; }

        /// <summary>
        /// the IDs of the set of tags that have been applied to a thread in a GUILD_FORUM or a GUILD_MEDIA channel; limited to 5
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ulong[]> AppliedTags { get; set; }

        internal InternalChannelEdit(string name)
        {
            Name = name;
        }

        private InternalChannelEdit() { }
    }
}
