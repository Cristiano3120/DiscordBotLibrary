using System.Text.Json.Serialization;

namespace DiscordBotLibrary.ActivityResources
{
    public readonly struct ActivityButton
    {
        [JsonPropertyName("label")]
        public string Label { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }
    }
}