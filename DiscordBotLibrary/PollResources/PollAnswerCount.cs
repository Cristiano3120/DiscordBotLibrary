namespace DiscordBotLibrary.PollResources
{
    public readonly struct PollAnswerCount
    {
        /// <summary>
        /// The answer_id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; init; }

        /// <summary>
        /// The number of votes for this answer
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; init; }

        /// <summary>
        /// Whether the current user voted for this answer
        /// </summary>
        [JsonProperty("me_voted")]
        public bool MeVoted { get; init; }
    }
}