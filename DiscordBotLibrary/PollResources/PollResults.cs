namespace DiscordBotLibrary.PollResources
{
    public sealed record PollResults
    {
        /// <summary>
        /// Whether the votes have been precisely counted
        /// </summary>
        [JsonPropertyName("is_finalized")]
        public bool IsFinalized { get; init; }

        /// <summary>
        /// The counts for each answer
        /// </summary>
        [JsonPropertyName("answer_counts")]
        public List<PollAnswerCount> AnswerCounts { get; init; } = default!;
    }
}