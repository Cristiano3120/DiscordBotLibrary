﻿using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public sealed class MediaChannelEdit : BaseChannelEdit
    {
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
        ///  Currently only REQUIRE_TAG (1 << 4) is supported for this type of channel
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ChannelFlags> Flags { get; set; }

        /// <summary>
        /// the set of tags that can be used in a GUILD_FORUM or a GUILD_MEDIA channel;
        /// <para>limited to 20</para>
        /// </summary>
        [JsonProperty("available_tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ForumTag> Tag { get; set; }

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
        public Optional<int> ThreadRateLimit { get; set; }

        /// <summary>
        /// the default sort order type used to order posts in GUILD_FORUM and GUILD_MEDIA channels
        /// </summary>
        [JsonProperty("default_sort_order", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<SortOrderType?> SortOrder { get; set; }

        internal MediaChannelEdit(string name) 
        { 
            Name = name;
        }

        private MediaChannelEdit() { }
    }
}
