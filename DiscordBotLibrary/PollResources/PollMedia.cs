namespace DiscordBotLibrary.PollResources
{
    public sealed record PollMedia
    {
        /// <summary>
        /// The text of the field
        /// </summary>
        [JsonProperty("text")]
        public string? Text { get; init; }

        /// <summary>
        /// The emoji of the field
        /// When creating a poll answer with an emoji, 
        /// one only needs to send either the id (custom emoji) or name (default emoji) as the only field.
        /// </summary>
        [JsonProperty("emoji")]
        public Emoji? Emoji { get; init; }
    }
}