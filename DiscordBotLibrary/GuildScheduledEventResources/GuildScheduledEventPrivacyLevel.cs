namespace DiscordBotLibrary.GuildScheduledEventResources
{
    /// <summary>
    /// Represents the privacy level of a scheduled event in a guild.
    /// </summary>
    public enum GuildScheduledEventPrivacyLevel : byte
    {
        /// <summary>
        /// The scheduled event is only accessible to guild members
        /// </summary>
        GuildOnly = 2,
    }
}