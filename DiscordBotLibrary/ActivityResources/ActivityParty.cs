using System.Text.Json.Serialization;

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
        [JsonPropertyName("id")]
        public string? Id { get; init; }

        /// <summary>
        /// Used to show the party's current and maximum size
        /// Contains two integers: current size and max size
        /// </summary>
        [JsonPropertyName("size")]
        public int[]? Size { get; init; }
    }
}