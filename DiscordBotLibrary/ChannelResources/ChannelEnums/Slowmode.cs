namespace DiscordBotLibrary.ChannelResources.ChannelEnums
{
    /// <summary>
    /// The dalay between messages in a channel, in seconds.
    /// </summary>
    public enum Slowmode : ushort
    {
        Off = 0,
        FiveSeconds = 5,
        TenSeconds = 10,
        FifteenSeconds = 15,
        ThirtySeconds = 30,
        OneMinute = 60,
        TwoMinutes = 120,
        FiveMinutes = 300,
        TenMinutes = 600,
        FifteenMinutes = 900,
        ThirtyMinutes = 1800,
        OneHour = 3600,
        TwoHours = 7200,
        SixHours = 21600,
    }
}
