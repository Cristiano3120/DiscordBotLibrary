namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents tags associated with a Discord role, providing metadata like bot ownership,
    /// integration origin, and special status like premium subscriber or purchasable roles.
    /// </summary>
    public sealed record RoleTags
    {
        /// <summary>
        /// The ID of the bot this role belongs to, if any.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("bot_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? BotId { get; init; }

        /// <summary>
        /// The ID of the integration this role belongs to, if any.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("integration_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? IntegrationId { get; init; }

        /// <summary>
        /// Whether this role is the guild's premium subscriber (Booster) role.
        /// This field is only present and set to null if true, and absent if false.
        /// </summary>
        [JsonProperty("premium_subscriber")]
        public object? PremiumSubscriber { get; init; }

        /// <summary>
        /// The ID of this role's subscription SKU and listing, if any.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("subscription_listing_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? SubscriptionListingId { get; init; }

        /// <summary>
        /// Whether this role is available for purchase. This field is only present and set to null if true.
        /// </summary>
        [JsonProperty("available_for_purchase")]
        public object? AvailableForPurchase { get; init; }

        /// <summary>
        /// Whether this role is associated with guild connections. Present and set to null if true.
        /// </summary>
        [JsonProperty("guild_connections")]
        public object? GuildConnections { get; init; }

        /// <summary>
        /// Indicates whether this role is a premium subscriber role.
        /// </summary>
        [JsonIgnore]
        public bool IsPremiumSubscriber => PremiumSubscriber is not null;

        /// <summary>
        /// Indicates whether this role is available for purchase.
        /// </summary>
        [JsonIgnore]
        public bool IsAvailableForPurchase => AvailableForPurchase is not null;

        /// <summary>
        /// Indicates whether this role has guild connections.
        /// </summary>
        [JsonIgnore]
        public bool HasGuildConnections => GuildConnections is not null;
    }

}
