using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.RoleResources
{
    public record RoleSubscriptionData
    {
        /// <summary>
        /// the id of the sku and listing that the user is subscribed to
        /// </summary>
        [JsonProperty("role_subscription_listing_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong RoleSubscriptionId { get; init; }

        /// <summary>
        /// the name of the role subscription tier.
        /// </summary>
        [JsonProperty("tier_name")]
        public string RoleSubscriptionName { get; init; } = string.Empty;

        /// <summary>
        /// the cumulative number of months that the user has been subscribed for
        /// </summary>
        [JsonProperty("total_months_subscribed")]
        public int TotalSubscriptionMonths { get; init; }

        [JsonProperty("is_renewal")]
        public bool IsRenewal { get; init; }
    }
}