namespace DiscordBotLibrary.PollResources
{
    public readonly struct PollAnswerCount
    {
        /// <summary>
        /// The answer_id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; init; }

        /// <summary>
        /// The number of votes for this answer
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; init; }

        /// <summary>
        /// Whether the current user voted for this answer
        /// </summary>
        [JsonPropertyName("me_voted")]
        public bool MeVoted { get; init; }
    }
}