namespace DiscordBotLibrary.GuildScheduledEventResources
{
    /// <summary>
    /// Represents the metadata for a scheduled event entity.
    /// </summary>
    public readonly struct GuildScheduledEventEntityMetadata
    {
        /// <summary>
        /// Location of the event (1-100 characters)
        /// Is required for EXTERNAL events.
        /// </summary>
        public string? Location { get; init; }
    }
}