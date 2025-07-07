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
        [JsonProperty("id")]
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// The guild id of the associated Stage channel
        /// </summary>
        [JsonProperty("guild_id")]
        public string GuildId { get; init; } = string.Empty;

        /// <summary>
        /// The ID of the associated Stage channel.
        /// </summary>
        [JsonProperty("channel_id")]
        public string ChannelId { get; init; } = string.Empty;

        /// <summary>
        /// The topic of the stage instance (1-120 characters).
        /// </summary>
        [JsonProperty("topic")]
        public string Topic { get; init; } = string.Empty;

        /// <summary>
        /// The <see cref="PrivacyLevel"/> of the Stage instance
        /// </summary>
        [JsonProperty("privacy_level")]
        public PrivacyLevel PrivacyLevel { get; init; }

        /// <summary>
        /// Whether or not Stage Discovery is disabled (deprecated)
        /// </summary>
        [JsonProperty("discoverable_disabled")]
        public bool DiscoverableDisabled { get; init; }

        /// <summary>
        /// The id of the scheduled event for this Stage instance
        /// </summary>
        [JsonProperty("guild_scheduled_event_id")]
        public string GuildScheduledEventId { get; init; } = string.Empty;
    }
}