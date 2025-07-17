namespace DiscordBotLibrary.ChannelResources.Channel
{
    /// <summary>
    /// Represents a Discord channel object.
    /// </summary>
    public sealed partial record Channel
    {
        #region Properties

        /// <summary>
        /// TYPE: Snowflake  
        /// The ID of this channel.
        /// </summary>
        public ulong Id { get; init; }

        /// <summary>
        /// The type of channel.
        /// </summary>
        public ChannelType Type { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// The ID of the guild (may be missing for some channel objects received over gateway guild dispatches).
        /// </summary>
        [JsonProperty]
        public ulong? GuildId { get; internal set; }

        /// <summary>
        /// Sorting position of the channel (channels with the same position are sorted by ID).
        /// </summary>
        public uint? Position { get; init; }

        /// <summary>
        /// Explicit permission overwrites for members and roles.
        /// </summary>
        public Overwrite[]? PermissionOverwrites { get; init; }

        /// <summary>
        /// The name of the channel (1–100 characters).
        /// <para><c>Null</c> when <see cref="Type"/> == <see cref="ChannelType.DM"/></para>
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// The channel topic.  
        /// For GUILD_FORUM and GUILD_MEDIA: 0–4096 characters.  
        /// For others: 0–1024 characters.
        /// </summary>
        public string? Topic { get; init; }

        /// <summary>
        /// Whether the channel is NSFW.
        /// </summary>
        public bool? Nsfw { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// The ID of the last message sent in this channel or thread.
        /// </summary>
        public ulong? LastMessageId { get; init; }

        /// <summary>
        /// The bitrate (in bits) of the voice channel.
        /// Can be beetwen 8k and 256k(384K on a hihgly boosted server)
        /// </summary>
        public uint? Bitrate { get; init; }

        /// <summary>
        /// The user limit of the voice channel.
        /// </summary>
        public uint? UserLimit { get; init; }

        /// <summary>
        /// Slowmode: How many seconds users must wait before sending another message.
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
        /// <para><c>Null</c> on some channel types</para>
        /// </summary>
        [JsonProperty("rate_limit_per_user")]
        public Slowmode? Slowmode { get; init; }

        /// <summary>
        /// The recipients of the DM (only present for DM/group DM channels).
        /// </summary>
        public User[]? Recipients { get; init; }

        /// <summary>
        /// Icon hash of the group DM.
        /// </summary>
        public string? Icon { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// ID of the creator of the group DM or thread.
        /// </summary>
        public ulong? OwnerId { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// Application ID of the group DM creator if bot-created.
        /// </summary>
        public ulong? ApplicationId { get; init; }

        /// <summary>
        /// Whether the channel is managed by an application via the gdm.join OAuth2 scope.
        /// </summary>
        public bool? Managed { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// For guild channels: ID of the parent category.  
        /// For threads: ID of the text channel this thread was created in.
        /// </summary>
        public ulong? ParentId { get; init; }

        /// <summary>
        /// ISO8601 timestamp of when the last pinned message was pinned.
        /// </summary>
        [JsonProperty]
        public DateTimeOffset? LastPinTimestamp { get; internal set; }

        /// <summary>
        /// Voice region ID for the voice channel
        /// </summary>
        [JsonIgnore]
        public RtcRegion RtcRegion => InternalRtcRegion ?? RtcRegion.Automatic;

        [JsonProperty("rtc_region")]
        public RtcRegion? InternalRtcRegion { get; init; }

        /// <summary>
        /// The camera video quality mode of the voice channel.
        /// </summary>
        public VideoQualityMode? VideoQualityMode { get; init; }

        /// <summary>
        /// Number of messages (excluding initial or deleted) in a thread.
        /// </summary>
        public uint? MessageCount { get; init; }

        /// <summary>
        /// Approximate count of users in a thread (stops at 50).
        /// </summary>
        public uint? MemberCount { get; init; }

        /// <summary>
        /// Thread-specific metadata.
        /// </summary>
        public ThreadMetadata? ThreadMetadata { get; init; }

        /// <summary>
        /// Thread member object for the current user, if joined.
        /// </summary>
        public ThreadMember? Member { get; init; }

        /// <summary>
        /// Default auto-archive duration (in minutes) for newly created threads.
        /// <para>Values: 60, 1440, 4320, 10080</para>
        /// </summary>
        public AutoArchiveDuration? DefaultAutoArchiveDuration { get; init; }

        /// <summary>
        /// Permissions that the invoking user has, only included in interactions.
        /// </summary>
        [JsonConverter(typeof(PermissionsConverter))]
        public DiscordPermissions? Permissions { get; init; }

        /// <summary>
        /// Bitfield of channel flags.
        /// </summary>
        public ChannelFlags? Flags { get; init; }

        /// <summary>
        /// Number of messages ever sent in a thread.
        /// </summary>
        public uint? TotalMessageSent { get; init; }

        /// <summary>
        /// Tags that can be used in GUILD_FORUM or GUILD_MEDIA channels.
        /// </summary>
        public ForumTag[]? AvailableTags { get; init; }

        /// <summary>
        /// IDs of the tags applied to a thread in GUILD_FORUM or GUILD_MEDIA.
        /// </summary>
        public ulong[]? AppliedTags { get; init; }

        /// <summary>
        /// The emoji shown on the “add reaction” button in GUILD_FORUM or GUILD_MEDIA threads.
        /// </summary>
        public DefaultReaction? DefaultReactionEmoji { get; init; }

        /// <summary>
        /// The time (in seconds) which the user has to wait before creating a new thread in GUILD_FORUM or GUILD_MEDIA.
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
        /// </summary>
        public Slowmode? DefaultThreadRateLimitPerUser { get; init; }

        /// <summary>
        /// Default sort order for posts in GUILD_FORUM or GUILD_MEDIA.
        /// </summary>
        public SortOrderType? DefaultSortOrder { get; init; }

        /// <summary>
        /// Default forum layout view used in GUILD_FORUM.
        /// </summary>
        public ForumLayoutType? DefaultForumLayout { get; init; }

        /// <summary>
        /// The users in the channel.
        /// Null if the channel is empty or not a voice channel.
        /// </summary>
        [JsonIgnore]
        public List<VoiceState>? VoiceStates { get; internal set; } = null;

        /// <summary>
        /// Only holds an valid count if the channel is of type voice channel.
        /// </summary>
        public int? CountOfUsersInVc => VoiceStates?.Count;

        #endregion

        /// <summary>
        /// <c>Null</c> if the request was invalid and <c>Message[]</c> otherwhise
        /// </summary>
        /// <returns></returns>
        public async Task<Message[]?> GetPinnedMessagesAsync()
        {
            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelEndpoint.Pins);
            return await DiscordClient.GetDiscordClient().RestApiLimiter.GetAsync<Message[]>(endpoint, CallerInfos.Create());
        }

        /// <summary>
        /// Deletes this channel permanently. <c>True</c> if the deletion was succesful.
        /// </summary>
        public async Task<bool> DeleteAsync()
        {
            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelEndpoint.Delete);
            return await DiscordClient.GetDiscordClient().RestApiLimiter.DeleteAsync(endpoint, CallerInfos.Create());
        }
    }
}