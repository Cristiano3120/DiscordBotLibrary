using System.Buffers.Text;

namespace DiscordBotLibrary.ChannelResources.PartialChannel
{
    public sealed partial record Channel
    {
        /// <summary>
        /// <c>SAFE: This method validates input before sending it</c>
        /// The Icon has to be binary encoded in base64
        /// <br></br>Can only be called on channels of type <see cref="ChannelType.GroupDM"/>
        /// </summary>
        /// <param name="icon"></param>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyIconAsync(string icon)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
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
        /// <br></br>Can only be called on channels of type <see cref="ChannelType.GroupDM"/>
        /// </summary>
        /// <returns>Null if the input was invalid and an Error log that gives detailed infos</returns>
        public async Task<Channel?> ModifyIconAsync(byte[] iconBytes)
        {
            if (!CheckPermissions(DiscordPermissions.ManageChannels))
            {
                LogInvalidInput(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageChannels), CallerInfos.Create());
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
    }
}
