namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents an emoji used in an activity.
    /// </summary>
    public readonly struct ActivityEmoji
    {
        /// <summary>
        /// Name of the emoji
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; }

        /// <summary>
        /// ID of the emoji (optional)
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; init; }

        /// <summary>
        /// Whether the emoji is animated (optional)
        /// </summary>
        [JsonProperty("animated")]
        public bool? Animated { get; init; }
    }

}