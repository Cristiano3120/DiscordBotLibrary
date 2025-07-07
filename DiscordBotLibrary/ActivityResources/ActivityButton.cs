namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents a button that is part of an activity.
    /// </summary>
    public readonly struct ActivityButton
    {
        [JsonProperty("label")]
        public string? Label { get; init; }

        [JsonProperty("url")]
        public string? Url { get; init; }
    }
}