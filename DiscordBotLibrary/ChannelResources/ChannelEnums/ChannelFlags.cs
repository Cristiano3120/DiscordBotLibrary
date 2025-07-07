namespace DiscordBotLibrary.ChannelResources.ChannelEnums
{
    /// <summary>
    /// Flags that specify the properties of a channel.
    /// </summary>
    [Flags]
    public enum ChannelFlags
    {
        None = 0,

        /// <summary>
        /// This thread is pinned to the top of its parent GUILD_FORUM or GUILD_MEDIA channel.
        /// </summary>
        Pinned = 1 << 1,

        /// <summary>
        /// A tag is required to be specified when creating a thread in a GUILD_FORUM or a GUILD_MEDIA channel.
        /// </summary>
        RequireTag = 1 << 4,

        /// <summary>
        /// When set hides the embedded media download options. Available only for media channels.
        /// </summary>
        HideMediaDownloadOptions = 1 << 15
    }
}
