namespace DiscordBotLibrary.ChannelResources
{
    /// <summary>
    /// Represents a Discord channel
    /// </summary>
    internal sealed record Channel
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = default!;

        [JsonPropertyName("type")]
        public ChannelType Type { get; init; }

        [JsonPropertyName("guild_id")]
        public string? GuildId { get; init; }

        [JsonPropertyName("position")]
        public int? Position { get; init; }

        [JsonPropertyName("permission_overwrites")]
        public List<Overwrite>? PermissionOverwrites { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("topic")]
        public string? Topic { get; init; }

        [JsonPropertyName("nsfw")]
        public bool? Nsfw { get; init; }

        [JsonPropertyName("last_message_id")]
        public string? LastMessageId { get; init; }

        [JsonPropertyName("bitrate")]
        public int? Bitrate { get; init; }

        [JsonPropertyName("user_limit")]
        public int? UserLimit { get; init; }

        [JsonPropertyName("rate_limit_per_user")]
        public int? RateLimitPerUser { get; init; }

        [JsonPropertyName("recipients")]
        public List<User>? Recipients { get; init; }

        [JsonPropertyName("icon")]
        public string? Icon { get; init; }

        [JsonPropertyName("owner_id")]
        public string? OwnerId { get; init; }

        [JsonPropertyName("application_id")]
        public string? ApplicationId { get; init; }

        [JsonPropertyName("managed")]
        public bool? Managed { get; init; }

        [JsonPropertyName("parent_id")]
        public string? ParentId { get; init; }

        [JsonPropertyName("last_pin_timestamp")]
        public DateTimeOffset? LastPinTimestamp { get; init; }

        [JsonPropertyName("rtc_region")]
        public string? RtcRegion { get; init; }

        [JsonPropertyName("video_quality_mode")]
        public VideoQualityMode? VideoQualityMode { get; init; }

        [JsonPropertyName("message_count")]
        public int? MessageCount { get; init; }

        [JsonPropertyName("member_count")]
        public int? MemberCount { get; init; }

        [JsonPropertyName("thread_metadata")]
        public ThreadMetadata? ThreadMetadata { get; init; }

        [JsonPropertyName("member")]
        public ThreadMember? Member { get; init; }

        [JsonPropertyName("default_auto_archive_duration")]
        public int? DefaultAutoArchiveDuration { get; init; }

        [JsonPropertyName("permissions")]
        public string? Permissions { get; init; }

        [JsonPropertyName("flags")]
        public ChannelFlags? Flags { get; init; }

        [JsonPropertyName("total_message_sent")]
        public int? TotalMessageSent { get; init; }

        [JsonPropertyName("available_tags")]
        public List<ForumTag>? AvailableTags { get; init; }

        [JsonPropertyName("applied_tags")]
        public List<string>? AppliedTags { get; init; }

        [JsonPropertyName("default_reaction_emoji")]
        public DefaultReaction? DefaultReactionEmoji { get; init; }

        [JsonPropertyName("default_thread_rate_limit_per_user")]
        public int? DefaultThreadRateLimitPerUser { get; init; }

        [JsonPropertyName("default_sort_order")]
        public SortOrderType? DefaultSortOrder { get; init; }

        [JsonPropertyName("default_forum_layout")]
        public ForumLayoutType? DefaultForumLayout { get; init; }

        [JsonPropertyName("available_locales")]
        public List<string>? AvailableLocales { get; init; }
    }
}
