using System.Buffers.Text;
using DiscordBotLibrary.ChannelResources.ChannelEditResources;
using DiscordBotLibrary.ChannelResources.ChannelEnums;

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
        public ulong Id { get; init; }

        /// <summary>
        /// The type of channel.
        /// </summary>
        public ChannelType Type { get; init; }

        /// <summary>
        /// TYPE: Snowflake  
        /// The ID of the guild (may be missing for some channel objects received over gateway guild dispatches).
        /// </summary>
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

        [JsonProperty("permissions")]
        internal ulong? InternalPermissions { get; init; }

        /// <summary>
        /// Permissions that the invoking user has, only included in interactions.
        /// </summary>
        [JsonIgnore]
        public DiscordPermissions? Permissions
            => InternalPermissions.HasValue
                ? (DiscordPermissions)InternalPermissions.Value
                : DiscordPermissions.None;

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
            return await DiscordClient.GetDiscordClient().RestApiLimiter.PatchAsync<T, Channel>(channelEdit, endpoint, CallerInfos.Create());
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
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }

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
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }

            if (!Base64.IsValid(icon) || string.IsNullOrEmpty(icon) || Type is not ChannelType.GroupDM)
            {
                LogInvalidInput("Only the Icon of a group Dm can be changed. Icon can´t be empty. " +
                    "Icon has to be encoded in base64", CallerInfos.Create());
                return null;
            }

            return await ModifyGroupDmAsync(x => x.Icon = icon);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// 
        /// </para>
        /// </summary>
        /// <param name="icon"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyIconAsync(byte[] iconBytes)
        {
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }

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
        /// <para>
        /// <c>This method can only be used on channels of type <see cref="ChannelType.Announcement"/> 
        /// and <see cref="ChannelType.Text"/></c>
        /// </para>
        /// </summary>
        /// <param name="icon"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyChannelTypeAsync(ChannelType type)
        {
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }


            if (type is not ChannelType.Text or ChannelType.Announcement)
            {
                LogInvalidInput($"The type {type} is not supported for this method. " +
                    $"Only {ChannelType.Text} and {ChannelType.Announcement} are supported", CallerInfos.Create());
                return null;
            }

            DiscordGuild? guild = DiscordClient.GetDiscordClient().GetGuild(GuildId!.Value);
            if (guild is null)
                return null;

            if (!guild.Features.Contains(GuildFeature.News))
            {
                LogInvalidInput($"The guild {guild.Name} does not support the {ChannelType.Announcement} type. " +
                    $"This is only supported on guilds with the {GuildFeature.News} feature", CallerInfos.Create());
                return null;
            }

            return await ModifyTextChannelAsync(x => x.Type = type);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <c>If the number is higher than </c>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyPositionAsync(uint position)
        {
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
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
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyTopicAsync(string topic)
        {
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }

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

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>This method can only NOT be used on channels of type thread and dm channels</c>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyNsfwAsync(bool nsfw)
        {
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }

            if (Type is not ChannelType.Text or ChannelType.Voice or ChannelType.Announcement 
                or ChannelType.StageVoice or ChannelType.Forum or ChannelType.Media)
            {
                LogInvalidInput($"Can´t use this method on this channel cause it´s of type {Type}." +
                    $"Look at the method docu to see what types can modify this property", CallerInfos.Create());
                return null;
            }

            return await ModifyAsync(x => x.Nsfw = nsfw);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>This method can only NOT be used on channels of type thread, dm channels and Announcement</c>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifySlowmodeAsync(Slowmode slowmode)
        {
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }

            if (Type is not ChannelType.Text or ChannelType.Voice or ChannelType.StageVoice 
                or ChannelType.Forum or ChannelType.Media)
            {
                LogInvalidInput($"Can´t use this method on this channel cause it´s of type {Type}." +
                   $"Look at the method docu to see what types can modify this property", CallerInfos.Create());
                return null;
            }

            return await ModifyAsync(x => x.Slowmode = slowmode);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>This method only works on channels of type <see cref="ChannelType.Voice"/> or <see cref="ChannelType.StageVoice"/></c>
        /// </para>
        /// 
        /// Bitrate can be between <c>8000</c> and <c>256000</c> bits per second (<c>384000</c> on a highly boosted server).
        /// If you set it to <c>0</c> it will be set to the default value of <c>64000</c>. Setting it below <c>8000</c> 
        /// will set it to <c>8000</c>. Setting it above the <c>maximum</c> will set it to the maximum value for the server.
        /// </summary>
        /// <returns><c>Null</c> if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyBitrateAsync(uint bitrate)
        {
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }

            if (Type is not ChannelType.Voice or ChannelType.StageVoice)
            {
                LogInvalidInput($"Can't use this method on this channel because it is of type {Type}. " +
                    $"Only Voice and StageVoice are supported.", CallerInfos.Create());
                return null;
            }

            const uint MIN_BITRATE = 8000;

            if (bitrate < MIN_BITRATE)
                bitrate = MIN_BITRATE;

            if (Type is ChannelType.Voice)
            {
                Guild guild = DiscordClient.GetDiscordClient().GetGuild(GuildId!.Value)!;

                bitrate = guild.PremiumTier switch
                {
                    PremiumTier.None => Math.Min(bitrate, 96000),
                    PremiumTier.Tier1 => Math.Min(bitrate, 128000),
                    PremiumTier.Tier2 => Math.Min(bitrate, 256000),
                    PremiumTier.Tier3 => Math.Min(bitrate, 384000),
                    _ => bitrate
                };
            }
            else
            {
                bitrate = Math.Min(bitrate, 64000);
            }

            return await ModifyAsync(x => x.Bitrate = bitrate);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>This method only works on channels of type <see cref="ChannelType.Voice"/> or <see cref="ChannelType.StageVoice"/></c>
        /// </para>
        /// Set it to <c>0</c> to remove the user limit.
        /// 
        /// <para>For <see cref="ChannelType.StageVoice"/> channels the max limit is <c>10_000</c></para>
        /// For <see cref="ChannelType.Voice"/> channels the max limit is <c>99</c>.
        /// </summary>
        /// <returns><c>Null</c> if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyUserLimitAsync(uint userLimit)
        {
            if (!CheckPermissions())
            {
                LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
                return null;
            }

            if (Type is not ChannelType.Voice and not ChannelType.StageVoice)
            {
                LogInvalidInput($"Can't use this method on this channel because it is of type {Type}. " +
                    $"Only Voice and StageVoice are supported.", CallerInfos.Create());
                return null;
            }

            userLimit = Type switch
            {
                ChannelType.Voice => Math.Min(userLimit, 99),
                ChannelType.StageVoice => Math.Min(userLimit, 10_000),
                _ => userLimit
            };

            return await ModifyAsync(x => x.UserLimit = userLimit);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// </summary>
        /// <returns><c>Null</c> if the input was invalid and an Error log that gives detailed infos</returns>
        //public async Task<Channel?> ModifyPermissionOverwritesAsync(IEnumerable<Overwrite> overwrites)
        //{
        //    if (!CheckPermissions())
        //    {
        //        LogInvalidInput("You need the ManageChannels permission to use this method", CallerInfos.Create());
        //        return null;
        //    }
        //}

        private static void LogInvalidInput(string msg, CallerInfos callerInfos)
        {
            DiscordClient.Logger.LogError($"[{callerInfos.CallerName}]: {msg}", CallerInfos.Create());
        }

        private bool CheckPermissions()
            => true;
        //=> !Permissions.HasValue || Permissions.Value.HasFlag(DiscordPermissions.ManageChannels);

        #endregion

        #endregion

        #region Voice
        public async Task<bool> JoinVoiceChannel(bool selfDeaf = false, bool selfMute = false)
        {
            if (!GuildId.HasValue)
            {
                DiscordClient.Logger.LogError("This channel is not a guild channel. " +
                    "You can only join voice channels in guilds.", CallerInfos.Create());
                return false;
            }

            DiscordGuild? guild = DiscordClient.GetDiscordClient().GetGuild(GuildId.Value);
            if (guild is null)
            {                 
                DiscordClient.Logger.LogError($"Guild with ID {GuildId.Value} not found.", CallerInfos.Create());
                return false;
            }

            return await guild.JoinVoiceChannelAsync(this, selfDeaf, selfMute);
        }

        public async Task LeaveVoiceChannel()
        {
            if (!GuildId.HasValue)
            {
                DiscordClient.Logger.LogError("This channel is not a guild channel. " +
                    "You can only leave voice channels in guilds.", CallerInfos.Create());
                return;
            }

            DiscordGuild? guild = DiscordClient.GetDiscordClient().GetGuild(GuildId.Value);
            if (guild is null)
            {
                DiscordClient.Logger.LogError($"Guild with ID {GuildId.Value} not found.", CallerInfos.Create());
                return;
            }

            await guild.LeaveVoiceChannelAsync();
        }


        #endregion

        /// <summary>
        /// Gets information about voice relevant data of a user in this channel.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VoiceState? GetVoiceState(ulong userId)
            => VoiceStates?.FirstOrDefault(x => x.UserId == userId);

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