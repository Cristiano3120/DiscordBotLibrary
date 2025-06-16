namespace DiscordBotLibrary.PollResources
{
    public sealed record PollAnswer
    {
        /// <summary>
        /// The ID of the answer
        /// </summary>
        [JsonPropertyName("answer_id")]
        public int AnswerId { get; init; }

        /// <summary>
        /// The data of the answer
        /// </summary>
        [JsonPropertyName("poll_media")]
        public PollMedia PollMedia { get; init; } = default!;
    }
}