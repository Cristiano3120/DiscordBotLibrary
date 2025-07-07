namespace DiscordBotLibrary.ChannelResources.ChannelEnums
{
    /// <summary>
    /// auto-archive duration (in minutes) for newly created threads.
    /// </summary>
    public enum AutoArchiveDuration : ushort
    {
        OneHour = 60,
        OneDay = 1440,
        ThreeDays = 4320,
        OneWeek = 10080
    }
}
