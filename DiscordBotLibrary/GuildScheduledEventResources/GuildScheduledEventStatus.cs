namespace DiscordBotLibrary.GuildScheduledEventResources
{
    /// <summary>
    /// Represents the status of a scheduled guild event.
    /// </summary>
    public enum GuildScheduledEventStatus : byte
    {
        /// <summary>
        /// The event is scheduled but has not started yet.
        /// </summary>
        Scheduled = 1,

        /// <summary>
        /// The event is currently active.
        /// </summary>
        Active = 2,

        /// <summary>
        /// The event has been completed and can no longer be updated.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// The event has been canceled and can no longer be updated.
        /// </summary>
        Canceled = 4
    }
}