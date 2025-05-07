namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents the timestamps of an activity.
    /// </summary>
    public readonly struct ActivityTimestamps
    {
        /// <summary>
        /// Gets the Unix timestamp (in milliseconds) when the activity started.
        /// </summary>
        [JsonPropertyName("start")]
        public int? Start { get; init; }

        /// <summary>
        /// Gets the Unix timestamp (in milliseconds) when the activity ends.
        /// </summary>
        [JsonPropertyName("end")]
        public int? End { get; init; }
    }
}