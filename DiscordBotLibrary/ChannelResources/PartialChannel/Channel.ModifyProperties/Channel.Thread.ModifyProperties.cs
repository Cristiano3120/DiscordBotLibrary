namespace DiscordBotLibrary.ChannelResources.PartialChannel
{
    public sealed partial record Channel
    {
        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on channels of type <see cref="ChannelType.PrivateThread"/>, <see cref="ChannelType.PublicThread"/> or <see cref="ChannelType.AnnouncementThread"/></c>
        /// <para><c> SETS ARCHIVED TO FALSE</c></para>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyArchivedAsync()
        {
            if (!CheckPermissions(DiscordPermissions.ManageThreads)
                || ThreadMetadata?.Locked == false && !CheckPermissions(DiscordPermissions.SendMessages))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageThreads), CallerInfos.Create());
                return null;
            }

            if (!IsThread(CallerInfos.Create()))
            {
                return null;
            }

            return ThreadMetadata?.Archived == true
                ? await ModifyAsync(x => x.Archived = false)
                : this;
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on channels of type <see cref="ChannelType.PrivateThread"/>, <see cref="ChannelType.PublicThread"/> or <see cref="ChannelType.AnnouncementThread"/></c>
        /// </para>
        /// <para>
        /// <c>ARCHIVED NEEDS TO BE FALSE. IF ARCHIVED == TRUE THIS REQUEST SETS IT TO FALSE</c>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyAutoArchiveDuration(AutoArchiveDuration autoArchiveDuration)
        {
            if (!CheckPermissions(DiscordPermissions.ManageThreads))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageThreads), CallerInfos.Create());
                return null;
            }

            if (!IsThread(CallerInfos.Create()))
            {
                return null;
            }

            if (autoArchiveDuration == ThreadMetadata?.AutoArchiveDuration)
            {
                CreateDuplicateValueError(nameof(ThreadMetadata.Value.AutoArchiveDuration), $"{autoArchiveDuration}");
                return this;
            }

            if (ThreadMetadata?.Archived == true)
            {
                return await ModifyAsync(x =>
                {
                    x.AutoArchiveDuration = autoArchiveDuration;
                    x.Archived = false;
                });
            }

            return await ModifyAsync(x => x.AutoArchiveDuration = autoArchiveDuration);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on channels of type <see cref="ChannelType.PrivateThread"/>, <see cref="ChannelType.PublicThread"/> or <see cref="ChannelType.AnnouncementThread"/></c>
        /// </para>
        /// <para>
        /// <c>ARCHIVED NEEDS TO BE FALSE. IF ARCHIVED == TRUE THIS REQUEST SETS IT TO FALSE</c>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyLockedAsync(bool locked)
        {
            if (!CheckPermissions(DiscordPermissions.ManageThreads))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageThreads), CallerInfos.Create());
                return null;
            }

            if (!IsThread(CallerInfos.Create()))
            {
                return null;
            }

            if (locked == ThreadMetadata?.Locked)
            {
                CreateDuplicateValueError(nameof(ThreadMetadata.Value.Locked), $"{locked}");
                return this;
            }

            if (ThreadMetadata?.Archived == true)
            {
                return await ModifyAsync(x =>
                { 
                    x.Locked = locked;
                    x.Archived = false;
                });
            }

            return await ModifyAsync(x => x.Locked = locked);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on channels of type <see cref="ChannelType.PrivateThread"/>, <see cref="ChannelType.PublicThread"/> or <see cref="ChannelType.AnnouncementThread"/></c>
        /// </para>
        /// <para>
        /// <c>ARCHIVED NEEDS TO BE FALSE. IF ARCHIVED == TRUE THIS REQUEST SETS IT TO FALSE</c>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyInvitableAsync(bool invitable)
        {
            if (!CheckPermissions(DiscordPermissions.ManageThreads))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageThreads), CallerInfos.Create());
                return null;
            }

            if (!IsThread(CallerInfos.Create()))
            {
                return null;
            }

            if (invitable == ThreadMetadata?.Invitable)
            {
                CreateDuplicateValueError(nameof(ThreadMetadata.Value.Invitable), $"{invitable}");
                return this;
            }

            if (ThreadMetadata?.Archived == true)
            {
                return await ModifyAsync(x =>
                {
                    x.Invitable = invitable;
                    x.Archived = false;
                });
            }

            return await ModifyAsync(x => x.Invitable = invitable);
        }

        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// <para>
        /// <c>It can only be used on channels of type <see cref="ChannelType.PrivateThread"/>, <see cref="ChannelType.PublicThread"/> or <see cref="ChannelType.AnnouncementThread"/></c>
        /// </para>
        /// <para>
        /// <c>ARCHIVED NEEDS TO BE FALSE. IF ARCHIVED == TRUE THIS REQUEST SETS IT TO FALSE</c>
        /// 
        /// <para>
        /// <c><see cref="ChannelFlags.Pinned"/> only works on <see cref="ChannelType.Forum"/> and <see cref="ChannelType.Media"/></c>
        /// <see cref="ChannelFlags.Pinned"/> will be ignored
        /// </para>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyThreadChannelFlagsAsync(ChannelFlags flags)
        {
            if (!CheckPermissions(DiscordPermissions.ManageThreads))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageThreads), CallerInfos.Create());
                return null;
            }

            if (!IsThread(CallerInfos.Create()))
            {
                return null;
            }

            if (flags == Flags)
            {
                int flagsAsBitset = (int)flags;
                CreateDuplicateValueError(nameof(Flags), $"{flagsAsBitset}");
                return this;
            }

            if (ParentId.HasValue)
            {
                DiscordGuild? guild = DiscordClient.GetDiscordClient().GetGuild(GuildId!.Value);
                if (flags.HasFlag(ChannelFlags.Pinned) 
                    && guild?.GetChannel(ParentId.Value)?.Type is not ChannelType.Forum or ChannelType.Media)
                {
                    Console.WriteLine("Removed the pinned flag cause ChannelFlags.Pinned is only allowed on Forum and Media channels");
                    flags &= ~ChannelFlags.Pinned;
                }
            }
            
            if (ThreadMetadata?.Archived == true)
            {
                return await ModifyAsync(x =>
                {
                    x.Flags = flags;
                    x.Archived = false;
                });
            }

            return await ModifyAsync(x => x.Flags = flags);
        }
    }
}
