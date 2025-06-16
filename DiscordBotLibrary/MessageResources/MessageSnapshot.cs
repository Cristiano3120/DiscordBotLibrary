namespace DiscordBotLibrary.MessageResources
{
    public record MessageSnapshot
    {
        /// <summary>
        /// Partial message object.
        /// <para>
        /// * The current subset of message fields consists of: type, content, embeds, attachments, timestamp, edited_timestamp, flags, mentions, mention_roles, stickers, sticker_items, and components.
        /// </para>
        /// </summary>
        public Message Message { get; init; }
    }
}