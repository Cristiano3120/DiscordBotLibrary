namespace DiscordBotLibrary.GuildScheduledEventResources
{
    /// <summary>
    /// Represents the type of a scheduled guild event entity.
    /// </summary>
    public enum GuildScheduledEventEntityType
    {
        /// <summary>
        /// The event is associated with a stage instance.
        /// </summary>
        StageInstance = 1,

        /// <summary>
        /// The event is associated with a voice channel.
        /// </summary>
        Voice = 2,

        /// <summary>
        /// The event is external and does not take place on Discord.
        /// </summary>
        External = 3
    }
}