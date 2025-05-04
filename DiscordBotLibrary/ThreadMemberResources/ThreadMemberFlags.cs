namespace DiscordBotLibrary.ThreadMemberResources
{
    [Flags]
    public enum ThreadMemberFlags : byte
    {
        None = 0,

        /// <summary>
        /// Suppress @mentions and notifications for this thread.
        /// </summary>
        SuppressNotifications = 1 << 0
    }
}
