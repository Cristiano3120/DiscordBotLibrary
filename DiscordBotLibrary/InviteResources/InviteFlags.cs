namespace DiscordBotLibrary.InviteResources
{
    [Flags]
    public enum InviteFlags
    {
        /// <summary>
        /// this invite is a guest invite for a voice channel
        /// </summary>
        IsGuestInvite = 1<<0,
    }
}