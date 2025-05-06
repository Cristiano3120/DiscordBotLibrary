namespace DiscordBotLibrary.StickerResources
{
    /// <summary>
    /// Represents the format type of a sticker in Discord.
    /// </summary>
    public enum StickerFormatType : byte
    {
        /// <summary>
        /// The sticker is in PNG format.
        /// </summary>
        Png = 1,
        /// <summary>
        /// The sticker is in APNG format.
        /// </summary>
        Apng = 2,
        /// <summary>
        /// The sticker is in Lottie format.
        /// </summary>
        Lottie = 3,

        /// <summary>
        /// The sticker is in GIF format (Animated).
        /// </summary>
        Gif = 4,
    }
}