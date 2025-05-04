using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    public record RoleTags
    {
        [JsonPropertyName("bot_id")]
        public string? BotId { get; set; }

        [JsonPropertyName("integration_id")]
        public string? IntegrationId { get; set; }

        [JsonPropertyName("premium_subscriber")]
        public object? PremiumSubscriber { get; set; }

        [JsonPropertyName("subscription_listing_id")]
        public string? SubscriptionListingId { get; set; }

        [JsonPropertyName("available_for_purchase")]
        public object? AvailableForPurchase { get; set; }

        [JsonPropertyName("guild_connections")]
        public object? GuildConnections { get; set; }
    }
}
