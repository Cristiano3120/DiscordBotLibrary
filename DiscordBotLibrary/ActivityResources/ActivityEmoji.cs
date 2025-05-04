using System.Text.Json.Serialization;

namespace DiscordBotLibrary.ActivityResources
{
    public readonly struct ActivityEmoji
    {
        /// <summary>
        /// Name of the emoji
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; }

        /// <summary>
        /// ID of the emoji (optional)
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; init; }

        /// <summary>
        /// Whether the emoji is animated (optional)
        /// </summary>
        [JsonPropertyName("animated")]
        public bool? Animated { get; init; }
    }

}