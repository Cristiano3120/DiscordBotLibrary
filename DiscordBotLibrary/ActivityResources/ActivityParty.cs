namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents a party in an activity
    /// </summary>
    public readonly struct ActivityParty
    {
        /// <summary>
        /// ID of the party
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; init; }

        /// <summary>
        /// Used to show the party's current and maximum size
        /// Contains two integers: [current size, max size]
        /// </summary>
        [JsonProperty("size")]
        public int[]? Size { get; init; }
    }
}