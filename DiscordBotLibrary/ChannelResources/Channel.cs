using System.Buffers.Text;
using System.Net.Http.Headers;
using DiscordBotLibrary.ChannelResources.ChannelEditResources;

namespace DiscordBotLibrary.ChannelResources
{
    /// <summary>
    /// Represents a Discord channel object.
    /// </summary>
    public sealed record Channel
    {
        #region Properties

        /// <summary>
        /// TYPE: Snowflake  
        /// The ID of this channel.
        /// </summary>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; }

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
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; init; }

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
        /// <para><c>Null</c> when <see cref="Type"/> == <see cref="ChannelType.DM"/></para>
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
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? LastMessageId { get; init; }

        /// <summary>
        /// The bitrate (in bits) of the voice channel.
        /// Can be beetwen 8k and 256k(384K on a hihgly boosted server)
        /// </summary>
        [JsonPropertyName("bitrate")]
        public int? Bitrate { get; init; }

        /// <summary>
        /// The user limit of the voice channel.
        /// </summary>
        [JsonPropertyName("user_limit")]
        public int? UserLimit { get; init; }

        /// <summary>
        /// Slowmode: How many seconds users must wait before sending another message.
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
        /// </summary>
        [JsonPropertyName("rate_limit_per_user")]
        public int? Slowmode { get; init; }

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
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? OwnerId { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// Application ID of the group DM creator if bot-created.
        /// </summary>
        [JsonPropertyName("application_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ApplicationId { get; init; }

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
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ParentId { get; init; }

        /// <summary>
        /// ISO8601 timestamp of when the last pinned message was pinned.
        /// </summary>
        [JsonPropertyName("last_pin_timestamp")]
        public DateTimeOffset? LastPinTimestamp { get; internal set; }

        /// <summary>
        /// Voice region ID for the voice channel
        /// </summary>
        [JsonIgnore]
        public RtcRegion RtcRegion => InternalRtcRegion ?? RtcRegion.Automatic;

        [JsonPropertyName("rtc_region")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RtcRegion? InternalRtcRegion { get; init; }

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
        /// <para>Values: 60, 1440, 4320, 10080</para>
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
        [JsonConverter(typeof(SnowflakeArrayConverter))]
        public ulong[]? AppliedTags { get; init; }

        /// <summary>
        /// The emoji shown on the “add reaction” button in GUILD_FORUM or GUILD_MEDIA threads.
        /// </summary>
        [JsonPropertyName("default_reaction_emoji")]
        public DefaultReaction? DefaultReactionEmoji { get; init; }

        /// <summary>
        /// Default duration (in seconds) that the clients will use for rate limiting thread creation
        /// <para>0, 5, 10, 15, 30, 60, 120, 300, 600, 900, 1800, 3600, 7200, 21600</para>
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
            return await DiscordClient.RestApiLimiter.GetAsync<Message[]>(endpoint, CallerInfos.Create());
        }

        /// <summary>
        /// Deletes this channel permanently. <c>True</c> if the deletion was succesful.
        /// </summary>
        public async Task<bool> DeleteAsync()
        {
            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelEndpoint.Delete);
            return await DiscordClient.RestApiLimiter.DeleteAsync(endpoint, CallerInfos.Create());
        }

        #region ModifyChannel
        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        /// </summary>
        /// <param name="channel"></param>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        private async Task<Channel?> ModifyAsync(Action<InternalChannelEdit> channelEditAction)
        {
            InternalChannelEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);

            return await ModifyChannelAsyncHelper(channelEdit);
        }

        #region ModifySpecificType

        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        /// </summary>
        /// <param name="channel"></param>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        public async Task<Channel?> ModifyVoiceChannelAsync(Action<VoiceChannelEdit> channelEditAction)
        {
            if (Type != ChannelType.Voice)
                throw new InvalidOperationException($"Type of this channel is {Type}. Call the Modify{Type}ChannelAsync method if it exists." +
                    $"If the method doesn´t exist channels of that type can´t be modified");

            VoiceChannelEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);
            return await ModifyChannelAsyncHelper(channelEdit);
        }

        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        /// </summary>
        /// <param name="channel"></param>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        public async Task<Channel?> ModifyTextChannelAsync(Action<TextChannelEdit> channelEditAction)
        {
            if (Type != ChannelType.Text)
                throw new InvalidOperationException($"Type of this channel is {Type}. Call the Modify{Type}ChannelAsync method if it exists." +
                    $"If the method doesn´t exist channels of that type can´t be modified");

            TextChannelEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);
            return await ModifyChannelAsyncHelper(channelEdit);
        }

        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        /// </summary>
        /// <param name="channel"></param>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        public async Task<Channel?> ModifyGroupDmAsync(Action<GroupDmEdit> channelEditAction)
        {
            if (Type != ChannelType.GroupDM)
                throw new InvalidOperationException($"Type of this channel is {Type}. Call the Modify{Type}ChannelAsync method if it exists." +
                    $"If the method doesn´t exist channels of that type can´t be modified");

            GroupDmEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);
            return await ModifyChannelAsyncHelper(channelEdit);
        }

        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        /// </summary>
        /// <param name="channel"></param>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        public async Task<Channel?> ModifyForumAsnyc(Action<ForumChannelEdit> channelEditAction)
        {
            if (Type != ChannelType.Forum)
                throw new InvalidOperationException($"Type of this channel is {Type}. Call the Modify{Type}ChannelAsync method if it exists." +
                    $"If the method doesn´t exist channels of that type can´t be modified");

            ForumChannelEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);
            return await ModifyChannelAsyncHelper(channelEdit);
        }

        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        /// </summary>
        /// <param name="channel"></param>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        public async Task<Channel?> ModifyMediaAsync(Action<MediaChannelEdit> channelEditAction)
        {
            if (Type != ChannelType.Media)
                throw new InvalidOperationException($"Type of this channel is {Type}. Call the Modify{Type}ChannelAsync method if it exists." +
                    $"If the method doesn´t exist channels of that type can´t be modified");

            MediaChannelEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);
            return await ModifyChannelAsyncHelper(channelEdit);
        }

        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        /// </summary>
        /// <param name="channel"></param>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        public async Task<Channel?> ModifyStageAsync(Action<StageChannelEdit> channelEditAction)
        {
            if (Type != ChannelType.StageVoice)
                throw new InvalidOperationException($"Type of this channel is {Type}. Call the Modify{Type}ChannelAsync method if it exists." +
                    $"If the method doesn´t exist channels of that type can´t be modified");

            StageChannelEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);
            return await ModifyChannelAsyncHelper(channelEdit);
        }

        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        /// </summary>
        /// <param name="channel"></param>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        public async Task<Channel?> ModifyAnnouncmentAsync(Action<AnnouncmentChannelEdit> channelEditAction)
        {
            if (Type != ChannelType.Announcement)
                throw new InvalidOperationException($"Type of this channel is {Type}. Call the Modify{Type}ChannelAsync method if it exists." +
                    $"If the method doesn´t exist channels of that type can´t be modified");

            AnnouncmentChannelEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);
            return await ModifyChannelAsyncHelper(channelEdit);
        }

        /// <summary>
        /// <para><c>UNSAFE: This method wont check if the set propertys are acceptable</c></para>
        /// Takes the channel with the property that you want to update. Use the <c>with</c> keyboard
        /// to change the propertys you wanna change. Some propertys cannot be changed tho.
        /// Changing those will lead to an error. If you dont know what propertys are changable use the other <c>overload</c> of this method
        ///<para>When setting archived to false, when locked is also false, only the SEND_MESSAGES permission is required.
        /// Otherwise, requires the MANAGE_THREADS permission.
        /// <para>Fires a Thread Update Gateway event. 
        /// Requires the thread to have archived set to false or be set to false in the request.</para></para>
        /// </summary>
        /// <returns><c>Null</c> if the request was unsuccesful</returns>
        public async Task<Channel?> ModifyThreadAsync(Action<ThreadChannelEdit> channelEditAction)
        {
            if (Type is not ChannelType.PublicThread or ChannelType.PrivateThread or ChannelType.AnnouncementThread)
                throw new InvalidOperationException($"Type of this channel is {Type}. Call the Modify{Type}ChannelAsync method if it exists." +
                    $"If the method doesn´t exist channels of that type can´t be modified");

            ThreadChannelEdit channelEdit = new(Name!);
            channelEditAction(channelEdit);
            return await ModifyChannelAsyncHelper(channelEdit);
        }

        private async Task<Channel?> ModifyChannelAsyncHelper<T>(T channelEdit)
        {
            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelEndpoint.Modify);
            return await DiscordClient.RestApiLimiter.PatchAsync<T, Channel>(channelEdit, endpoint, CallerInfos.Create());
        }

        #endregion

        #region ModifyProperties

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// 
        /// <para>
        /// You can only modify the name of a channel 2 times per 10 minutes. 
        /// This method will look out for that.
        /// </para>
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyNameAsync(string name)
        {
            if (name.Length is < 1 or > 100)
            {
                LogInvalidInput($"The length of the param {nameof(name)} has to be greater than 1 and less than 100", CallerInfos.Create());
                return null;
            }

            return await ModifyAsync(x => x.Name = name);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// The Icon has to be binary encoded in base64
        /// </summary>
        /// <param name="icon"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyIconAsync(string icon)
        {
            if (!Base64.IsValid(icon) || string.IsNullOrEmpty(icon) || Type is not ChannelType.GroupDM)
            {
                LogInvalidInput("TOnly the Icon of a group Dm can be changed. Icon can´t be empty. " +
                    "Icon has to be encoded in base64", CallerInfos.Create());
                return null;
            }

            return await ModifyGroupDmAsync(x => x.Icon = icon);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// </summary>
        /// <param name="icon"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyIconAsync(byte[] iconBytes)
        {
            if (iconBytes.Length == 0 || Type is not ChannelType.GroupDM)
            {
                LogInvalidInput("Only the Icon of a group Dm can be changed. Icon can´t be empty.", CallerInfos.Create());
                return null;
            }

            string base64Icon = Convert.ToBase64String(iconBytes);
            return await ModifyGroupDmAsync(x => x.Icon = base64Icon);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// </summary>
        /// <param name="icon"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyChannelTypeAsync(ChannelType type)
        {
            if (type is not ChannelType.Text or ChannelType.Announcement)
            {
                LogInvalidInput($"The type {type} is not supported for this method. " +
                    $"Only {ChannelType.Text} and {ChannelType.Announcement} are supported", CallerInfos.Create());
                return null;
            }

            return await ModifyTextChannelAsync(x => x.Type = type);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// </summary>
        /// <param name="icon"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyPositionAsync(int position)
        {
            if (position < 0)
            {
                LogInvalidInput($"The position {position} is not valid. It has to be greater than or equal to 0", CallerInfos.Create());
                return null;
            }

            return await ModifyAsync(x => x.Position = position);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// Supports <c>markdown</c> formatting. This property can only be changed twice per 10 minutes.
        /// </para>
        /// <para>
        /// This method modifies the topic of a channel.
        /// This only works for channels of type <see cref="ChannelType.Text"/>, <see cref="ChannelType.Announcement"/>,
        /// <see cref="ChannelType.Forum"/> and <see cref="ChannelType.Media"/>.
        /// <para>
        /// 
        /// </para>
        /// 
        /// Topic can be between 0 and 1024 characters long for channels 
        /// of type <see cref="ChannelType.Text"/> or <see cref="ChannelType.Announcement"/>.
        /// </para>
        ///
        /// <para>
        /// Topic can be between 0 and 4096 characters long for channels 
        /// of type <see cref="ChannelType.Forum"/> or <see cref="ChannelType.Media"/>.
        /// </para>
        /// 
        /// Set it to <c>0</c> to remove the topic.
        /// </summary>
        /// <param name="icon"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyTopicAsync(string topic)
        {
            DiscordClient.Logger.LogInfo($"Changing the topic of the {Type}channel: {Name} to {topic}");

            int length = topic.Length;
            if (length > 4096)
            {
                LogInvalidInput($"The length of the param {nameof(topic)} must be ≤ 4096 (or ≤ 1024 for certain types)."
                    , CallerInfos.Create());
                return null;
            }

            bool isValidType = Type is ChannelType.Text or ChannelType.Announcement or ChannelType.Forum or ChannelType.Media;
            if (!isValidType)
            {
                LogInvalidInput($"This {Type} is not supported for this method. Look at the method docu for more info"
                    , CallerInfos.Create());
                return null;
            }

            bool isTextOrAnnouncement = Type is ChannelType.Text or ChannelType.Announcement;
            if (isTextOrAnnouncement && length > 1024)
            {
                LogInvalidInput($"This {Type} only supports a topic of up to 1024 chars. Look at the method docu for more info"
                    , CallerInfos.Create());
                return null;
            }

            return await ModifyAsync(x => x.Topic = topic);
        }

        private static void LogInvalidInput(string msg, CallerInfos callerInfos)
        {
            DiscordClient.Logger.LogError($"[{callerInfos.CallerName}]: " +
                $"{msg}");
        }

        #endregion

        #endregion
    }
}