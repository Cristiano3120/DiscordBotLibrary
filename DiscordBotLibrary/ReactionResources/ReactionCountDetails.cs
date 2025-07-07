namespace DiscordBotLibrary.ReactionResources
{
    public readonly struct ReactionCountDetails
    {
        /// <summary>
        /// Count of super reactions
        /// </summary>
        [JsonProperty("burst")]
        public int Burst { get; init; }

        /// <summary>
        /// Count of normal reactions
        /// </summary>
        [JsonProperty("normal")]
        public int Normal { get; init; }
    }
}
