namespace DiscordBotLibrary.GuildScheduledEventResources
{
    /// <summary>
    /// Represents a scheduled event in a guild.
    /// </summary>
    public record GuildScheduledEvent
    {
        /// <summary>
        /// Gets the ID of the scheduled event.
        /// TYPE: Snoflake
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// Gets the ID of the guild to which the scheduled event belongs.
        /// TYPE: Snoflake
        /// </summary>
        [JsonProperty("guild_id")]
        public string GuildId { get; init; } = string.Empty;

        /// <summary>
        /// Gets the ID of the channel where the scheduled event will be hosted, or null if the entity type is EXTERNAL.
        /// TYPE: Snoflake
        /// </summary>
        [JsonProperty("channel_id")]
        public string? ChannelId { get; init; }

        /// <summary>
        /// Gets the ID of the user who created the scheduled event. May be null for events created before October 25th, 2021.
        /// TYPE: Snoflake
        /// </summary>
        [JsonProperty("creator_id")]
        public string? CreatorId { get; init; }

        /// <summary>
        /// Gets the name of the scheduled event (1-100 characters).
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Gets the description of the scheduled event (1-1000 characters), or null if none is set.
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; init; }

        /// <summary>
        /// Gets the time the scheduled event will start.
        /// </summary>
        [JsonProperty("scheduled_start_time")]
        public DateTimeOffset ScheduledStartTime { get; init; }

        /// <summary>
        /// Gets the time the scheduled event will end. Required if the entity type is EXTERNAL.
        /// </summary>
        [JsonProperty("scheduled_end_time")]
        public DateTimeOffset? ScheduledEndTime { get; init; }

        /// <summary>
        /// Gets the privacy level of the scheduled event.
        /// </summary>
        [JsonProperty("privacy_level")]
        public GuildScheduledEventPrivacyLevel PrivacyLevel { get; init; }

        /// <summary>
        /// Gets the status of the scheduled event.
        /// </summary>
        [JsonProperty("status")]
        public GuildScheduledEventStatus Status { get; init; }

        /// <summary>
        /// Gets the type of the scheduled event.
        /// </summary>
        [JsonProperty("entity_type")]
        public GuildScheduledEventEntityType EntityType { get; init; }

        /// <summary>
        /// Gets the ID of the entity associated with the scheduled event, or null if none.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("entity_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? EntityId { get; init; }

        /// <summary>
        /// Gets additional metadata for the scheduled event, or null if none.
        /// </summary>
        [JsonProperty("entity_metadata")]
        public GuildScheduledEventEntityMetadata? EntityMetadata { get; init; }

        /// <summary>
        /// Gets the user who created the scheduled event. Will be null for events created before October 25th, 2021.
        /// </summary>
        [JsonProperty("creator")]
        public User? Creator { get; init; }

        /// <summary>
        /// Gets the number of users subscribed to the scheduled event.
        /// </summary>
        [JsonProperty("user_count")]
        public int? UserCount { get; init; }

        /// <summary>
        /// Gets the cover image hash of the scheduled event, or null if not set.
        /// </summary>
        [JsonProperty("image")]
        public string? Image { get; init; }

        /// <summary>
        /// Gets the recurrence rule for the event, if it is recurring.
        /// </summary>
        [JsonProperty("recurrence_rule")]
        public GuildScheduledEventRecurrenceRule? RecurrenceRule { get; init; }
    }
}
