namespace DiscordBotLibrary.ChannelResources.PartialChannel
{
    public sealed partial record Channel
    {
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

        private static void LogInvalidInput(string msg, CallerInfos callerInfos)
            => DiscordClient.Logger.CustomLog(ConsoleColor.Red, LogLevel.Error, $"[{callerInfos.CallerName}]: {msg}");

        /// <returns>{<paramref name="propertyName"/>} already has the value: {<paramref name="value"/>}</returns>
        private static string CreateDuplicateValueError(string propertyName, string value)
            => $"{propertyName} already has the value: {value}";
        
        private bool ValidateCreateInviteParams(CreateInviteParams createInviteParams, CallerInfos callerInfos)
        {
            const uint MAX_AGE = 604_800;      // 7 DAYS
            const byte MAX_USES = 100;

            if (createInviteParams.MaxAge.HasValue)
            {
                createInviteParams.MaxAge = Math.Min(createInviteParams.MaxAge.Value, MAX_AGE);
            }

            if (createInviteParams.MaxUses.HasValue)
            {
                createInviteParams.MaxUses = Math.Min(createInviteParams.MaxUses.Value, MAX_USES);
            }

            if (createInviteParams.TargetType.Value is InviteTargetType targetType)
            {
                switch (targetType)
                {
                    case InviteTargetType.Stream:
                        if (!createInviteParams.TargetUserId.HasValue)
                        {
                            DiscordClient.Logger.LogError("TargetUserId can't be null if TargetType is set to Stream.", callerInfos);
                            return false;
                        }

                        bool isStreaming = VoiceStates?.FirstOrDefault(v => v.UserId == createInviteParams.TargetUserId.Value)?.SelfStream == true;
                        if (!isStreaming)
                        {
                            DiscordClient.Logger.LogError("Target user must be streaming if TargetType is Stream.", callerInfos);
                            return false;
                        }
                        break;

                    case InviteTargetType.EmbeddedApplication:
                        if (!createInviteParams.TargetApplicationId.HasValue)
                        {
                            DiscordClient.Logger.LogError("TargetApplicationId can't be null if InviteTargetType is EmbeddedApplication.", callerInfos);
                            return false;
                        }
                        break;
                }
            }

            return true;
        }

        #region Permissions

        private bool CheckPermissions(DiscordPermissions permissions)
        {
            DiscordClient client = DiscordClient.GetDiscordClient();
            DiscordGuild? guild = client.GetGuild(GuildId!.Value);

            if (Permissions.HasValue)
            {
                return Permissions.Value.HasFlag(permissions)
                    || Permissions.Value.HasFlag(DiscordPermissions.Administrator);
            }

            User? currentUser = client.CurrentUser;
            if (currentUser is null)
            {
                return false;
            }

            ulong[]? userRoleIds = guild?.GetMember(currentUser.Id)?.Roles;

            if (userRoleIds is null or { Length: 0 })
                return false;

            bool canManageChannels = false;
            foreach (ulong roleId in userRoleIds)
            {
                Role? role = guild?.GetRole(roleId);
                if (role is null)
                    continue;

                if (role.Permissions.HasFlag(DiscordPermissions.Administrator))
                {
                    return true;
                }
                else if (role.Permissions.HasFlag(permissions))
                {
                    canManageChannels = true;
                    break;
                }
            }

            return CheckPermissionOverwrites(GuildId.Value, canManageChannels, userRoleIds, currentUser.Id);
        }

        private bool CheckPermissionOverwrites(ulong everyoneRoleId, bool canManageChannel
            , ulong[] userRoleIds, ulong currentUserId)
        {
            DiscordPermissions allow = 0;
            DiscordPermissions deny = 0;

            if (PermissionOverwrites is null or { Length: 0 })
                return canManageChannel;

            foreach (Overwrite overwrite in PermissionOverwrites)
            {
                if (overwrite.Id == everyoneRoleId)
                {
                    allow |= overwrite.Allow;
                    deny |= overwrite.Deny;
                }
            }

            foreach (Overwrite overwrite in PermissionOverwrites)
            {
                if (userRoleIds.Contains(overwrite.Id))
                {
                    allow |= overwrite.Allow;
                    deny |= overwrite.Deny;
                }
            }

            foreach (Overwrite overwrite in PermissionOverwrites)
            {
                if (overwrite.Id == currentUserId)
                {
                    allow = allow & ~overwrite.Deny | overwrite.Allow;
                    deny = (deny & ~overwrite.Allow) | overwrite.Deny;
                }
            }

            if (allow == 0 && deny == 0)
                return canManageChannel;

            return allow.HasFlag(DiscordPermissions.ManageChannels)
                && !deny.HasFlag(DiscordPermissions.ManageChannels);
        }

        private string GetMissingPermissionsErrorMsg(DiscordPermissions permissions)
            => $"You need the {permissions} permission to use this method";

        #endregion

        #region IsChannelType

        /// <summary>
        /// Logs an error message if <c>false</c>
        /// </summary>
        /// <returns></returns>
        private bool IsGuildChannel(CallerInfos callerInfos, bool log = true)
        { 
            bool isGuildChannel = Type is not ChannelType.DM and not ChannelType.GroupDM; 
            if (!isGuildChannel && log)
            {
                LogInvalidInput("Channel has to be a guild channel to execute this method", callerInfos);
            }

            return isGuildChannel;
        }

        /// <summary>
        /// Logs an error message if <c>false</c>
        /// </summary>
        /// <returns></returns>
        internal bool IsVoiceChannel(CallerInfos callerInfos)
        {
            bool isVoiceChannel = Type is ChannelType.Voice or ChannelType.StageVoice;
            if (!isVoiceChannel)
            {
                LogInvalidInput($"Channel has to be of type {ChannelType.Voice} or {ChannelType.StageVoice} to execute this method"
                    , callerInfos);
            }

            return isVoiceChannel;
        }

        /// <summary>
        /// Logs an error message if <c>false</c>
        /// </summary>
        /// <returns></returns>
        private bool IsTextOrAnnouncement(CallerInfos callerInfos)
        {
            bool isTextOrAnnouncement = Type is ChannelType.Announcement or ChannelType.Text;
            if (!isTextOrAnnouncement)
            {
                LogInvalidInput($"Channel has to be of type {ChannelType.Announcement} or {ChannelType.Text} to execute this method"
                    , callerInfos);
            }

            return !isTextOrAnnouncement;
        }

        /// <summary>
        /// Logs an error message if <c>false</c>
        /// </summary>
        /// <returns></returns>
        private bool IsMediaOrForum(CallerInfos callerInfos)
        {
            bool isMediaOrForum = Type is ChannelType.Forum or ChannelType.Media;
            if (!isMediaOrForum)
            {
                LogInvalidInput($"Channel has to be of type {ChannelType.Announcement} or {ChannelType.Text} to execute this method", callerInfos);
            }

            return !isMediaOrForum;
        }

        /// <summary>
        /// Logs an error message if <c>false</c> 
        /// <para>
        /// Checks if channel is of type <see cref="ChannelType.Announcement"/>, <see cref="ChannelType.Text"/>
        /// , <see cref="ChannelType.Media"/> or <see cref="ChannelType.Forum"/>
        /// </para>
        /// </summary>
        private bool IsTextBased(CallerInfos callerInfos)
        {
            bool isTextBased = Type is ChannelType.Forum or ChannelType.Media or ChannelType.Text or ChannelType.Announcement;
            if (!isTextBased)
            {
                LogInvalidInput($"Channel has to be of type {ChannelType.Announcement}, {ChannelType.Text}" +
                    $", {ChannelType.Forum} or {ChannelType.Media} to execute this method", callerInfos);
            }

            return isTextBased;
        }

        private bool IsThread(CallerInfos callerInfos, bool log = true)
        {
            bool isThread = Type is ChannelType.AnnouncementThread or ChannelType.PrivateThread or ChannelType.PublicThread;
            if (!isThread && log)
            {
                LogInvalidInput($"Channel has to be of type {ChannelType.PrivateThread}, {ChannelType.PublicThread}" +
                    $"or {ChannelType.AnnouncementThread} to execute this method", callerInfos);
            }

            return isThread;
        }

        #endregion
    }
}
