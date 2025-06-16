namespace DiscordBotLibrary.MessageResources
{
    [Flags]
    public enum MessageAttachmentFlags
    {
        /// <summary>
        /// this attachment has been edited using the remix feature on mobile
        /// </summary>
        IsRemix = 1 << 2,
    }
}