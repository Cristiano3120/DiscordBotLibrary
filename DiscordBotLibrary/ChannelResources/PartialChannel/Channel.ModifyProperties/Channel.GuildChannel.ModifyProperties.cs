namespace DiscordBotLibrary.ChannelResources.PartialChannel
{
    public sealed partial record Channel
    {
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
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsTextOrAnnouncement(CallerInfos.Create()))
            {
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
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
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
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
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
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
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
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsVoiceChannel(CallerInfos.Create()))
            {
                return null;
            }

            const uint MIN_BITRATE = 8000;
            const uint DEFAULT_BITRATE = 64_000;
            const uint MAX_BITRATE_NO_BOOST = 96_000;
            const uint MAX_BITRATE_TIER1_BOOST = 128_000;
            const uint MAX_BITRATE_TIER2_BOOST = 256_000;
            const uint MAX_BITRATE_TIER3_BOOST = 384_000;

            if (bitrate < MIN_BITRATE)
                bitrate = MIN_BITRATE;

            if (Type is ChannelType.Voice)
            {
                Guild guild = DiscordClient.GetDiscordClient().GetGuild(GuildId!.Value)!;

                bitrate = guild.PremiumTier switch
                {
                    PremiumTier.None => Math.Min(bitrate, MAX_BITRATE_NO_BOOST),
                    PremiumTier.Tier1 => Math.Min(bitrate, MAX_BITRATE_TIER1_BOOST),
                    PremiumTier.Tier2 => Math.Min(bitrate, MAX_BITRATE_TIER2_BOOST),
                    PremiumTier.Tier3 => Math.Min(bitrate, MAX_BITRATE_TIER3_BOOST),
                    _ => bitrate
                };
            }
            else //if Type is ChannelType.StageVoice
            {
                bitrate = Math.Min(bitrate, DEFAULT_BITRATE);
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
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsVoiceChannel(CallerInfos.Create()))
            {
                return null;
            }

            const uint MAX_VC_USER_LIMIT = 99;
            const uint MAX_STAGE_VC_USER_LIMIT = 10_000;

            userLimit = Type switch
            {
                ChannelType.Voice => Math.Min(userLimit, MAX_VC_USER_LIMIT),
                ChannelType.StageVoice => Math.Min(userLimit, MAX_STAGE_VC_USER_LIMIT),
                _ => userLimit
            };

            return await ModifyAsync(x => x.UserLimit = userLimit);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It works on every type of Guild Channel.</c> <br></br>
        /// This method allows you to make specific channel rules for a user or a role.
        /// This means that you can for example say that a specific role can´t edit this channel. <br></br>
        /// <c>Look at the second code example to do that</c>
        /// <code>
        /// <c>HOW TO REMOVE ALL PERMISSION OVERWRITES:</c> <para></para>
        /// [Call the method without any param]
        /// await <see cref="ModifyPermissionOverwritesAsync(Overwrite[])"/>;
        /// 
        /// <para></para>
        /// <c>HOW TO ADD A PERMISSION OVERWRITE:</c> <br></br>
        /// <see cref="Overwrite"/> overwrite = new()
        /// {
        ///     Id = 1341855039748309033,
        ///     Deny = DiscordPermissions.ManageChannels,
        ///     Type = OverwriteType.Role,
        /// };
        /// <para></para>
        /// [Pass that <see cref="Overwrite"/> obj per param]
        /// await <see cref="ModifyPermissionOverwritesAsync(Overwrite[])"/>;
        /// </code>
        /// </para>
        ///</summary>
        public async Task<Channel?> ModifyPermissionOverwritesAsync(params Overwrite[] overwrites)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels) || !CheckPermissions(DiscordPermissions.ManageRoles))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            return await ModifyAsync(x => x.PermissionOverwrites = overwrites);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on Guild channels</c>
        /// Pass in null to remove this channel of its category
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyCategoryAsync(ulong? categoryId)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsGuildChannel(CallerInfos.Create()))
            {
                return null;
            }

            if (ParentId is null && categoryId is null)
            {
                LogInvalidInput("Channel already doesn´t have a category", CallerInfos.Create());
                return this;
            }

            if (ParentId == categoryId)
            {
                string errorMsg = CreateDuplicateValueError(nameof(ParentId), $"{categoryId}");
                LogInvalidInput(errorMsg, CallerInfos.Create());
                return this;
            }

            if (categoryId is null)
            {
                return await ModifyAsync(x => x.ParentCategory = categoryId);
            }

            DiscordClient client = DiscordClient.GetDiscordClient();
            DiscordGuild? guild = client.GetGuild(GuildId!.Value);
            Channel? channel = guild?.GetChannel(categoryId.Value); 

            if (channel is null or not { Type: ChannelType.Category})
            {
                LogInvalidInput("The categoryId you gave via param is invalid. " +
                    "The channel must be part of the Guild and of type: Category", CallerInfos.Create());
                return null;
            }

            return await ModifyAsync(x => x.ParentCategory = categoryId);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Voice"/> and <see cref="ChannelType.StageVoice"/></c>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyRtcRegionAsync(RtcRegion rtcRegion)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsVoiceChannel(CallerInfos.Create()))
            {
                return null;
            }

            if (rtcRegion == RtcRegion)
            {
                string errorMsg = CreateDuplicateValueError(nameof(RtcRegion), $"{rtcRegion}");
                LogInvalidInput(errorMsg, CallerInfos.Create());
                return this;
            }

            return rtcRegion == RtcRegion.Automatic 
                ? await ModifyAsync(x => x.RtcRegion = null) 
                : await ModifyAsync(x => x.RtcRegion = rtcRegion);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Voice"/> and <see cref="ChannelType.StageVoice"/></c>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyVideoQualityModeAsync(VideoQualityMode videoQualityMode)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsVoiceChannel(CallerInfos.Create()))
            {
                return null;
            }

            if (VideoQualityMode == videoQualityMode)
            {
                string errorMsg = CreateDuplicateValueError(nameof(VideoQualityMode), $"{videoQualityMode}");
                LogInvalidInput(errorMsg, CallerInfos.Create());
                return this;
            }

            return await ModifyAsync(x => x.VideoQualityMode = videoQualityMode);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Text"/>, <see cref="ChannelType.Announcement"/>
        /// , <see cref="ChannelType.Forum"/> or <see cref="ChannelType.Media"/></c>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyDefaultArchieveDurationAsync(AutoArchiveDuration autoArchiveDuration)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsTextBased(CallerInfos.Create()))
            {
                return null;
            }

            if (autoArchiveDuration == DefaultAutoArchiveDuration)
            {
                string errorMsg = CreateDuplicateValueError(nameof(DefaultAutoArchiveDuration), $"{autoArchiveDuration}");
                LogInvalidInput(errorMsg, CallerInfos.Create());
                return this;
            }

            return await ModifyAsync(x => x.AutoArchiveDuration = autoArchiveDuration);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Media"/> or <see cref="ChannelType.Forum"/>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyChannelFlagsAsync(ChannelFlags flags)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsMediaOrForum(CallerInfos.Create()))
            {
                return null;
            }

            if (flags == Flags)
            {
                int flagsAsBitset = (int)flags;
                CreateDuplicateValueError(nameof(Flags), $"{flagsAsBitset}");
                return this;
            }

            return await ModifyAsync(x => x.Flags = flags);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Media"/> or <see cref="ChannelType.Forum"/>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyAvaliableTagsAsync(params ForumTag[] availableForumTags)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            return !IsMediaOrForum(CallerInfos.Create()) 
                ? null 
                : await ModifyAsync(x => x.AvailableTags = availableForumTags);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Media"/> or <see cref="ChannelType.Forum"/>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyDefaultReactionEmojiAsync(DefaultReaction defaultReaction)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsMediaOrForum(CallerInfos.Create()))
            {
                return null;
            }

            if (DefaultReactionEmoji == defaultReaction)
            {
                CreateDuplicateValueError(nameof(defaultReaction), $"{defaultReaction}");
                return this;
            }

            return await ModifyAsync(x => x.DefaultReaction = defaultReaction);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Media"/>, <see cref="ChannelType.Forum"/> 
        /// or <see cref="ChannelType.Text"/> </c>
        /// </para>
        /// </summary>
        /// <returns><c>Null</c> if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyThreadRateLimitPerUserAsync(Slowmode slowmode)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (Type is not ChannelType.Text or ChannelType.Forum or ChannelType.Media)
            {
                LogInvalidInput($"Channel has to be of type {ChannelType.Text}, {ChannelType.Forum} or {ChannelType.Media}" +
                    $" to execute this method", CallerInfos.Create());
                return null;
            }

            if (DefaultThreadRateLimitPerUser == slowmode)
            {
                CreateDuplicateValueError(nameof(DefaultThreadRateLimitPerUser), $"{slowmode}");
                return this;
            }

            return await ModifyAsync(x => x.ThreadRateLimit = slowmode);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Media"/> or <see cref="ChannelType.Forum"/> </c>
        /// </para>
        /// </summary>
        /// <returns><c>Null</c> if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifySortOrderAsync(SortOrderType sortOrderType)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (!IsMediaOrForum(CallerInfos.Create()))
            {
                return null;    
            }

            if (DefaultSortOrder == sortOrderType)
            {
                CreateDuplicateValueError(nameof(DefaultSortOrder), $"{sortOrderType}");
                return this;
            }

            return await ModifyAsync(x => x.SortOrder = sortOrderType);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on <see cref="ChannelType.Forum"/> </c>
        /// </para>
        /// </summary>
        /// <returns><c>Null</c> if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyForumLayout(ForumLayoutType layoutType)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
                return null;
            }

            if (Type is not ChannelType.Forum)
            {
                LogInvalidInput("This channel needs to be of type Forum", CallerInfos.Create());
                return null;
            }

            if (layoutType == DefaultForumLayout)
            {
                CreateDuplicateValueError(nameof(DefaultForumLayout), $"{layoutType}");
                return this;
            }

            return await ModifyAsync(x => x.LayoutType = layoutType);
        }
    }
}
