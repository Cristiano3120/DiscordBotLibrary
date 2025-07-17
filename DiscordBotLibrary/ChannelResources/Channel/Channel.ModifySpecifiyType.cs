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
    }
}
