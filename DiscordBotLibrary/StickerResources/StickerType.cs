namespace DiscordBotLibrary.StickerResources
{
    /// <summary>
    /// Represents the type of a sticker in Discord.
    /// </summary>
    public enum StickerType : byte
    {
        /// <summary>
        /// An official sticker in a pack
        /// </summary>
        Standard = 1,
        /// <summary>
        /// A sticker uploaded to a guild for the guild's members
        /// </summary>
        Guild = 2,
    }
}