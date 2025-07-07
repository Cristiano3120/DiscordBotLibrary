namespace DiscordBotLibrary.PollResources
{
    public sealed record Poll
    {
        /// <summary>
        /// The question of the poll. Only text is supported.
        /// </summary>
        [JsonProperty("question")]
        public PollMedia Question { get; init; } = default!;

        /// <summary>
        /// Each of the answers available in the poll.
        /// </summary>
        [JsonProperty("answers")]
        public List<PollAnswer> Answers { get; init; } = default!;

        /// <summary>
        /// The time when the poll ends.
        /// </summary>
        [JsonProperty("expiry")]
        public DateTimeOffset? Expiry { get; init; }

        /// <summary>
        /// Whether a user can select multiple answers
        /// </summary>
        [JsonProperty("allow_multiselect")]
        public bool AllowMultiselect { get; init; }

        /// <summary>
        /// The layout type of the poll
        /// </summary>
        [JsonProperty("layout_type")]
        public PollLayoutType LayoutType { get; init; }

        /// <summary>
        /// The results of the poll
        /// </summary>
        [JsonProperty("results")]
        public PollResults? Results { get; init; }
    }
}