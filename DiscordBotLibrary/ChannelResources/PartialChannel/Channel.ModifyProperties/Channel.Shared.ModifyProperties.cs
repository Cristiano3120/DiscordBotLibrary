namespace DiscordBotLibrary.ChannelResources.PartialChannel
{
    public sealed partial record Channel
    {
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
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
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
        /// <para>
        /// <c>This method can only be used on channels of type: <see cref="ChannelType.Text"/>, <see cref="ChannelType.Voice"/>/></c>
        /// , <see cref="ChannelType.StageVoice"/>, <see cref="ChannelType.Forum"/>, <see cref="ChannelType.Media"/>
        /// , <see cref="ChannelType.PublicThread"/>, <see cref="ChannelType.PrivateThread"/> or <see cref="ChannelType.AnnouncementThread"/>
        /// </para>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifySlowmodeAsync(Slowmode slowmode)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                string errorMsg = Type is not ChannelType.AnnouncementThread or ChannelType.PrivateThread or ChannelType.PublicThread
                    ? GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels) 
                    : GetMissingPermissionsErrorMsg(DiscordPermissions.ManageThreads);
                LogInvalidInput(errorMsg, CallerInfos.Create());
                return null;
            }

            if (Type is not ChannelType.Text or ChannelType.Voice or ChannelType.StageVoice
                or ChannelType.Forum or ChannelType.Media 
                or ChannelType.PrivateThread or ChannelType.PublicThread or ChannelType.AnnouncementThread)
            {
                LogInvalidInput($"Can´t use this method on this channel cause it´s of type {Type}." +
                   $"Look at the method docu to see what types can modify this property", CallerInfos.Create());
                return null;
            }

            return await ModifyAsync(x => x.Slowmode = slowmode);
        }
    }
}
