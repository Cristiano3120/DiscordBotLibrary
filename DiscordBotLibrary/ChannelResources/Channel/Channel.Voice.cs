namespace DiscordBotLibrary.ChannelResources.Channel
{
    public sealed partial record Channel
    {
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

        /// <summary>
        /// Gets information about voice relevant data of a user in this channel.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VoiceState? GetVoiceState(ulong userId)
            => VoiceStates?.FirstOrDefault(x => x.UserId == userId);
    }
}
