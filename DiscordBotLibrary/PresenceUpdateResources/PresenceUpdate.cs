using System.Text.Json.Serialization;
using DiscordBotLibrary.UserResources;
using Activity = DiscordBotLibrary.ActivityResources.Activity;

namespace DiscordBotLibrary.PresenceUpdateResources
{
    /// <summary>
    /// Represents a user's presence update within a guild.
    /// </summary>
    public sealed record PresenceUpdate
    {
        /// <summary>
        /// Gets the user whose presence is being updated.
        /// </summary>
        [JsonPropertyName("user")]
        public User User { get; init; } = null!;

        /// <summary>
        /// Gets the ID of the guild where the presence update occurred.
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string GuildId { get; init; } = string.Empty;

        /// <summary>
        /// Gets the user's current status. 
        /// </summary>
        [JsonPropertyName("status")]
        public PresenceStatus Status { get; init; }

        /// <summary>
        /// Gets the list of the user's current activities.
        /// </summary>
        [JsonPropertyName("activities")]
        public Activity[] Activities { get; init; } = [];

        /// <summary>
        /// Gets the user's status per platform (desktop, mobile, web).
        /// </summary>
        [JsonPropertyName("client_status")]
        public ClientStatus ClientStatus { get; init; }
    }
}