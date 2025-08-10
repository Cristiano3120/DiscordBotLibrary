namespace DiscordBotLibrary.ChannelResources.PartialChannel
{
    public sealed partial record Channel
    {
        /// <summary>
        /// Joins the voice channel.
        /// </summary>
        /// <returns></returns>
        public async Task JoinAsync(bool selfDeaf = false, bool selfMute = false)
        {
            if (!IsVoiceChannel(CallerInfos.Create()))
            {
                return;
            }

            DiscordGuild? guild = DiscordClient.GetDiscordClient().GetGuild(GuildId!.Value);
            if (guild is null)
            {
                DiscordClient.Logger.LogError($"Guild with ID {GuildId.Value} not found.", CallerInfos.Create());
                return;
            }

            await DiscordClient
                .GetDiscordClient()
                .VoiceChannelHandler
                .ConnectToVcAsync(GuildId.Value, Id, selfDeaf, selfMute);
        }

        public async Task LeaveAsync()
        {
            if (!IsVoiceChannel(CallerInfos.Create()))
            {
                return;
            }

            DiscordGuild? guild = DiscordClient.GetDiscordClient().GetGuild(GuildId!.Value);
            if (guild is null)
            {
                DiscordClient.Logger.LogError($"Guild with ID {GuildId.Value} not found.", CallerInfos.Create());
                return;
            }

            await DiscordClient
                .GetDiscordClient()
                .VoiceChannelHandler
                .ConnectToVcAsync(GuildId.Value, Id);
        }

        /// <summary>
        /// Gets information about voice relevant data of a user in this channel.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VoiceState? GetVoiceState(ulong userId)
            => VoiceStates?.FirstOrDefault(x => x.UserId == userId);

        public async Task<bool> MoveUserAsync(ulong userId, Channel targetChannel)
        {
            if (!CheckPermissions(DiscordPermissions.MoveMembers))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.MoveMembers), CallerInfos.Create());
                return false;
            }

            if (!targetChannel.CheckPermissions(DiscordPermissions.Connect))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.Connect), CallerInfos.Create());
                return false;
            }

            string endpoint = $"guilds/{GuildId}/members/{userId}";
            ModifyMember modifyMember = new()
            {
                ChannelId = targetChannel.Id
            };

            GuildMember? guildMember = await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .PatchAsync<ModifyMember, GuildMember>(modifyMember, endpoint, CallerInfos.Create());

            return guildMember is not null;
        }

        public async Task<bool> DisconnectUserAsync(ulong userId)
        {
            if (!CheckPermissions(DiscordPermissions.MoveMembers))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.MoveMembers), CallerInfos.Create());
                return false;
            }

            if (!CheckPermissions(DiscordPermissions.Connect))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.Connect), CallerInfos.Create());
                return false;
            }

            string endpoint = $"guilds/{GuildId}/members/{userId}";
            ModifyMember modifyMember = new()
            {
                ChannelId = null
            };

            GuildMember? guildMember = await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .PatchAsync<ModifyMember, GuildMember>(modifyMember, endpoint, CallerInfos.Create());

            return guildMember is not null;
        }
    }
}
