namespace DiscordBotLibrary.ReactionResources
{
    public record Reaction
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("count_details")]
        public ReactionCountDetails CountDetails { get; init; } = default!;

        /// <summary>
        /// Whether the current user reacted using this emoji
        /// </summary>
        [JsonPropertyName("me")]
        public bool Me { get; init; }

        [JsonPropertyName("me_burst")]
        public bool MeBurst { get; init; }

        /// <summary>
        /// Partial emoji object for the reaction
        /// </summary>
        [JsonPropertyName("emoji")]
        public Emoji Emoji { get; init; } = default!;

        /// <summary>
        /// HEX colors used for super reaction
        /// </summary>
        public string[] BurstColors { get; init; } = [];
    }
}