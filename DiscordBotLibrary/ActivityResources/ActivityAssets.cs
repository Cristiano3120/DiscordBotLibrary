using System.Text.Json.Serialization;

namespace DiscordBotLibrary.ActivityResources
{
    public record ActivityAssets
    {
        /// <summary>
        /// See Activity Asset Image
        /// </summary>
        [JsonPropertyName("large_image")]
        public string? LargeImage { get; init; }

        /// <summary>
        /// Text displayed when hovering over the large image of the activity
        /// </summary>
        [JsonPropertyName("large_text")]
        public string? LargeText { get; init; }

        /// <summary>
        /// See Activity Asset Image
        /// </summary>
        [JsonPropertyName("small_image")]
        public string? SmallImage { get; init; }

        /// <summary>
        /// Text displayed when hovering over the small image of the activity
        /// </summary>
        [JsonPropertyName("small_text")]
        public string? SmallText { get; init; }
    }

}