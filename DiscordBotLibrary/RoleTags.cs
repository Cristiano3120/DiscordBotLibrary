using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    internal record RoleTags
    {
        [JsonPropertyName("bot_id")]
        public string? BotId { get; init; }

        [JsonPropertyName("integration_id")]
        public string? IntegrationId { get; init; }

        [JsonPropertyName("premium_subscriber")]
        public bool? PremiumSubscriber { get; init; }

        [JsonPropertyName("subscription_listing_id")]
        public string? SubscriptionListingId { get; init; }

        [JsonPropertyName("available_for_purchase")]
        public bool? AvailableForPurchase { get; init; }

        [JsonPropertyName("guild_connections")]
        public object? GuildConnections { get; init; }
    }
}
