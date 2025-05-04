namespace DiscordBotLibrary.StageInstanceResources
{
    /// <summary>
    /// Represents the privacy level of a Stage instance.
    /// </summary>
    public enum PrivacyLevel : byte
    {
        /// <summary>
        /// The Stage instance is visible publicly. (deprecated)
        /// </summary>
        Public = 1,

        /// <summary>
        /// The Stage instance is only visible to members of the guild.
        /// </summary>
        GuildOnly = 2
    }
}
