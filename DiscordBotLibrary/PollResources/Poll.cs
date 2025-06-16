namespace DiscordBotLibrary.PollResources
{
    public sealed record Poll
    {
        /// <summary>
        /// The question of the poll. Only text is supported.
        /// </summary>
        [JsonPropertyName("question")]
        public PollMedia Question { get; init; } = default!;

        /// <summary>
        /// Each of the answers available in the poll.
        /// </summary>
        [JsonPropertyName("answers")]
        public List<PollAnswer> Answers { get; init; } = default!;

        /// <summary>
        /// The time when the poll ends.
        /// </summary>
        [JsonPropertyName("expiry")]
        public DateTimeOffset? Expiry { get; init; }

        /// <summary>
        /// Whether a user can select multiple answers
        /// </summary>
        [JsonPropertyName("allow_multiselect")]
        public bool AllowMultiselect { get; init; }

        /// <summary>
        /// The layout type of the poll
        /// </summary>
        [JsonPropertyName("layout_type")]
        public PollLayoutType LayoutType { get; init; }

        /// <summary>
        /// The results of the poll
        /// </summary>
        [JsonPropertyName("results")]
        public PollResults? Results { get; init; }
    }
}