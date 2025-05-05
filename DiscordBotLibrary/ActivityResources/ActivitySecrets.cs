using System.Text.Json.Serialization;

namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents the secrets for an activity.
    /// </summary>
    public readonly struct ActivitySecrets
    {
        [JsonPropertyName("join")]
        public string? Join { get; init; }

        [JsonPropertyName("spectate")]
        public string? Spectate { get; init; }

        [JsonPropertyName("match")]
        public string? Match { get; init; }
    }
}