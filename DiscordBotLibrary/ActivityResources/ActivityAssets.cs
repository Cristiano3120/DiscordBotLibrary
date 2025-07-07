namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents the assets of an activity.
    /// </summary>
    public sealed record ActivityAssets
    {
        /// <summary>
        /// See Activity Asset Image
        /// </summary>
        [JsonProperty("large_image")]
        public string? LargeImage { get; init; }

        /// <summary>
        /// Text displayed when hovering over the large image of the activity
        /// </summary>
        [JsonProperty("large_text")]
        public string? LargeText { get; init; }

        /// <summary>
        /// See Activity Asset Image
        /// </summary>
        [JsonProperty("small_image")]
        public string? SmallImage { get; init; }

        /// <summary>
        /// Text displayed when hovering over the small image of the activity
        /// </summary>
        [JsonProperty("small_text")]
        public string? SmallText { get; init; }
    }

}