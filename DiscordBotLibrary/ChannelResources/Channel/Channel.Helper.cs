namespace DiscordBotLibrary.ChannelResources.Channel
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

        private bool CanManageChannel(CallerInfos callerInfos)
        {
            DiscordClient client = DiscordClient.GetDiscordClient();
            DiscordGuild? guild = client.GetGuild(GuildId!.Value);

            if (Permissions.HasValue)
            {
                return Permissions.Value.HasFlag(DiscordPermissions.ManageChannels)
                    || Permissions.Value.HasFlag(DiscordPermissions.Administrator);
            }

            User? currentUser = client.CurrentUser;
            if (currentUser is null)
            {
                DiscordClient.Logger.LogError("DiscordClient.CurrentUser is null", callerInfos);
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
                else if (role.Permissions.HasFlag(DiscordPermissions.ManageChannels))
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
                    deny = deny & ~overwrite.Allow | overwrite.Deny;
                }
            }

            if (allow == 0 && deny == 0)
                return canManageChannel;

            return allow.HasFlag(DiscordPermissions.ManageChannels)
                && !deny.HasFlag(DiscordPermissions.ManageChannels);
        }
    }
}
