namespace DiscordBotLibrary.ReactionResources
{
    public record Reaction
    {
        [JsonProperty("count")]
        public int Count { get; init; }

        [JsonProperty("count_details")]
        public ReactionCountDetails CountDetails { get; init; } = default!;

        /// <summary>
        /// Whether the current user reacted using this emoji
        /// </summary>
        [JsonProperty("me")]
        public bool Me { get; init; }

        [JsonProperty("me_burst")]
        public bool MeBurst { get; init; }

        /// <summary>
        /// Partial emoji object for the reaction
        /// </summary>
        [JsonProperty("emoji")]
        public Emoji Emoji { get; init; } = default!;

        /// <summary>
        /// HEX colors used for super reaction
        /// </summary>
        public string[] BurstColors { get; init; } = [];
    }
}