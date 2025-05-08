namespace DiscordBotLibrary.ChannelResources
{
    /// <summary>
    /// Represents a Discord channel object.
    /// </summary>
    public sealed record Channel
    {
        /// <summary>
        /// TYPE: Snowflake  
        /// The ID of this channel.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; init; } = default!;

        /// <summary>
        /// The type of channel.
        /// </summary>
        [JsonPropertyName("type")]
        public ChannelType Type { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// The ID of the guild (may be missing for some channel objects received over gateway guild dispatches).
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string? GuildId { get; init; }

        /// <summary>
        /// Sorting position of the channel (channels with the same position are sorted by ID).
        /// </summary>
        [JsonPropertyName("position")]
        public int? Position { get; init; }

        /// <summary>
        /// Explicit permission overwrites for members and roles.
        /// </summary>
        [JsonPropertyName("permission_overwrites")]
        public Overwrite[]? PermissionOverwrites { get; init; }

        /// <summary>
        /// The name of the channel (1–100 characters).
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        /// <summary>
        /// The channel topic.  
        /// For GUILD_FORUM and GUILD_MEDIA: 0–4096 characters.  
        /// For others: 0–1024 characters.
        /// </summary>
        [JsonPropertyName("topic")]
        public string? Topic { get; init; }

        /// <summary>
        /// Whether the channel is NSFW.
        /// </summary>
        [JsonPropertyName("nsfw")]
        public bool? Nsfw { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// The ID of the last message sent in this channel or thread.
        /// </summary>
        [JsonPropertyName("last_message_id")]
        public string? LastMessageId { get; init; }

        /// <summary>
        /// The bitrate (in bits) of the voice channel.
        /// </summary>
        [JsonPropertyName("bitrate")]
        public int? Bitrate { get; init; }

        /// <summary>
        /// The user limit of the voice channel.
        /// </summary>
        [JsonPropertyName("user_limit")]
        public int? UserLimit { get; init; }

        /// <summary>
        /// Slowmode: How many seconds users must wait before sending another message (0–21600).
        /// </summary>
        [JsonPropertyName("rate_limit_per_user")]
        public int? RateLimitPerUser { get; init; }

        /// <summary>
        /// The recipients of the DM (only present for DM/group DM channels).
        /// </summary>
        [JsonPropertyName("recipients")]
        public List<User>? Recipients { get; init; }

        /// <summary>
        /// Icon hash of the group DM.
        /// </summary>
        [JsonPropertyName("icon")]
        public string? Icon { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// ID of the creator of the group DM or thread.
        /// </summary>
        [JsonPropertyName("owner_id")]
        public string? OwnerId { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// Application ID of the group DM creator if bot-created.
        /// </summary>
        [JsonPropertyName("application_id")]
        public string? ApplicationId { get; init; }

        /// <summary>
        /// Whether the channel is managed by an application via the gdm.join OAuth2 scope.
        /// </summary>
        [JsonPropertyName("managed")]
        public bool? Managed { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// For guild channels: ID of the parent category.  
        /// For threads: ID of the text channel this thread was created in.
        /// </summary>
        [JsonPropertyName("parent_id")]
        public string? ParentId { get; init; }

        /// <summary>
        /// ISO8601 timestamp of when the last pinned message was pinned.
        /// </summary>
        [JsonPropertyName("last_pin_timestamp")]
        public DateTimeOffset? LastPinTimestamp { get; init; }

        /// <summary>
        /// Voice region ID for the voice channel (automatic when null).
        /// </summary>
        [JsonPropertyName("rtc_region")]
        public string? RtcRegion { get; init; }

        /// <summary>
        /// The camera video quality mode of the voice channel.
        /// </summary>
        [JsonPropertyName("video_quality_mode")]
        public VideoQualityMode? VideoQualityMode { get; init; }

        /// <summary>
        /// Number of messages (excluding initial or deleted) in a thread.
        /// </summary>
        [JsonPropertyName("message_count")]
        public int? MessageCount { get; init; }

        /// <summary>
        /// Approximate count of users in a thread (stops at 50).
        /// </summary>
        [JsonPropertyName("member_count")]
        public int? MemberCount { get; init; }

        /// <summary>
        /// Thread-specific metadata.
        /// </summary>
        [JsonPropertyName("thread_metadata")]
        public ThreadMetadata? ThreadMetadata { get; init; }

        /// <summary>
        /// Thread member object for the current user, if joined.
        /// </summary>
        [JsonPropertyName("member")]
        public ThreadMember? Member { get; init; }

        /// <summary>
        /// Default auto-archive duration (in minutes) for newly created threads.
        /// </summary>
        [JsonPropertyName("default_auto_archive_duration")]
        public int? DefaultAutoArchiveDuration { get; init; }

        /// <summary>
        /// Computed permissions for the invoking user, only included in interactions.
        /// </summary>
        [JsonPropertyName("permissions")]
        public string? Permissions { get; init; }

        /// <summary>
        /// Bitfield of channel flags.
        /// </summary>
        [JsonPropertyName("flags")]
        public ChannelFlags? Flags { get; init; }

        /// <summary>
        /// Number of messages ever sent in a thread.
        /// </summary>
        [JsonPropertyName("total_message_sent")]
        public int? TotalMessageSent { get; init; }

        /// <summary>
        /// Tags that can be used in GUILD_FORUM or GUILD_MEDIA channels.
        /// </summary>
        [JsonPropertyName("available_tags")]
        public List<ForumTag>? AvailableTags { get; init; }

        /// <summary>
        /// IDs of the tags applied to a thread in GUILD_FORUM or GUILD_MEDIA.
        /// </summary>
        [JsonPropertyName("applied_tags")]
        public List<string>? AppliedTags { get; init; }

        /// <summary>
        /// The emoji shown on the “add reaction” button in GUILD_FORUM or GUILD_MEDIA threads.
        /// </summary>
        [JsonPropertyName("default_reaction_emoji")]
        public DefaultReaction? DefaultReactionEmoji { get; init; }

        /// <summary>
        /// Default rate_limit_per_user copied to new threads.
        /// </summary>
        [JsonPropertyName("default_thread_rate_limit_per_user")]
        public int? DefaultThreadRateLimitPerUser { get; init; }

        /// <summary>
        /// Default sort order for posts in GUILD_FORUM or GUILD_MEDIA.
        /// </summary>
        [JsonPropertyName("default_sort_order")]
        public SortOrderType? DefaultSortOrder { get; init; }

        /// <summary>
        /// Default forum layout view used in GUILD_FORUM.
        /// </summary>
        [JsonPropertyName("default_forum_layout")]
        public ForumLayoutType? DefaultForumLayout { get; init; }
    }
}
