namespace DiscordBotLibrary.RoleResources
{
    public record RoleSubscriptionData
    {
        /// <summary>
        /// the id of the sku and listing that the user is subscribed to
        /// </summary>
        [JsonPropertyName("role_subscription_listing_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong RoleSubscriptionId { get; init; }

        /// <summary>
        /// the name of the role subscription tier.
        /// </summary>
        [JsonPropertyName("tier_name")]
        public string RoleSubscriptionName { get; init; } = string.Empty;

        /// <summary>
        /// the cumulative number of months that the user has been subscribed for
        /// </summary>
        [JsonPropertyName("total_months_subscribed")]
        public int TotalSubscriptionMonths { get; init; }

        [JsonPropertyName("is_renewal")]
        public bool IsRenewal { get; init; }
    }
}