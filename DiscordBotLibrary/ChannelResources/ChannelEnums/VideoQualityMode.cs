namespace DiscordBotLibrary.ChannelResources.ChannelEnums
{
    /// <summary>
    /// Represents the video quality mode of a <see cref="Channel"/>.
    /// </summary>
    public enum VideoQualityMode : byte
    {
        /// <summary>
        /// Discord chooses the quality for optimal performance
        /// </summary>
        Auto = 1,
        /// <summary>
        /// 720p video quality mode.
        /// </summary>
        Full = 2
    }
}