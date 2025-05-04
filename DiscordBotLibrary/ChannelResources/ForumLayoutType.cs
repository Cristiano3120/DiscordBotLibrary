namespace DiscordBotLibrary.ChannelResources
{
    public enum ForumLayoutType : byte
    {
        /// <summary>
        /// No default has been set for forum channel
        /// </summary>
        NotSet = 0,
        /// <summary>
        /// Display posts as a list
        /// </summary>
        ListView = 1,
        /// <summary>
        /// Display posts as a collection of tiles
        /// </summary>
        GalleryView = 2,
    }
}