namespace DiscordBotLibrary.ReactionResources
{
    public readonly struct ReactionCountDetails
    {
        /// <summary>
        /// Count of super reactions
        /// </summary>
        [JsonPropertyName("burst")]
        public int Burst { get; init; }

        /// <summary>
        /// Count of normal reactions
        /// </summary>
        [JsonPropertyName("normal")]
        public int Normal { get; init; }
    }
}
