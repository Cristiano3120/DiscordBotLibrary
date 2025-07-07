namespace DiscordBotLibrary.ChannelResources.ChannelEnums
{
    /// <summary>
    /// Represents the sort order for forum posts in a Discord channel.
    /// </summary>
    public enum SortOrderType : byte
    {
        /// <summary>
        /// Sort forum posts by activity
        /// </summary>
        LatestActivity = 0,
        /// <summary>
        /// Sort forum posts by creation time (from most recent to oldest)
        /// </summary>
        CreationDate = 1,
    }
}