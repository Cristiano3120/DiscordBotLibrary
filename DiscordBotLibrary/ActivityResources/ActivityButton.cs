using System.Text.Json.Serialization;

namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents a button that is part of an activity.
    /// </summary>
    public readonly struct ActivityButton
    {
        [JsonPropertyName("label")]
        public string Label { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }
    }
}