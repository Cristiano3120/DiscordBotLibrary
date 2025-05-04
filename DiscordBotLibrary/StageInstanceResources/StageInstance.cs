using System.Text.Json.Serialization;

namespace DiscordBotLibrary.StageInstanceResources
{
    /// <summary>
    /// A Stage Instance holds information about a live stage
    /// </summary>
    public record StageInstance
    {
        /// <summary>
        /// The ID of the stage instance.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// The guild id of the associated Stage channel
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string GuildId { get; init; } = string.Empty;

        /// <summary>
        /// The ID of the associated Stage channel.
        /// </summary>
        [JsonPropertyName("channel_id")]
        public string ChannelId { get; init; } = string.Empty;

        /// <summary>
        /// The topic of the stage instance (1-120 characters).
        /// </summary>
        [JsonPropertyName("topic")]
        public string Topic { get; init; } = string.Empty;

        /// <summary>
        /// The <see cref="PrivacyLevel"/> of the Stage instance
        /// </summary>
        [JsonPropertyName("privacy_level")]
        public PrivacyLevel PrivacyLevel { get; init; }

        /// <summary>
        /// Whether or not Stage Discovery is disabled (deprecated)
        /// </summary>
        [JsonPropertyName("discoverable_disabled")]
        public bool DiscoverableDisabled { get; init; }

        /// <summary>
        /// The id of the scheduled event for this Stage instance
        /// </summary>
        [JsonPropertyName("guild_scheduled_event_id")]
        public string GuildScheduledEventId { get; init; } = string.Empty;
    }
}