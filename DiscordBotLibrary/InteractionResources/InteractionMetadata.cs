namespace DiscordBotLibrary.InteractionResources
{
    public class InteractionMetadata
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; }

        [JsonPropertyName("type")]
        public InteractionType InteractionType { get; init; }

        /// <summary>
        /// User who triggered the interaction
        /// </summary>
        [JsonPropertyName("user")]
        public User User { get; init; } = default!;

        [JsonPropertyName("authorizing_integration_owners")]
        [JsonConverter(typeof(SnowflakeDictConverter<ApplicationIntegrationType, ulong>))]
        public Dictionary<ApplicationIntegrationType, ulong> AuthorizingIntegrationOwners { get; init; } = [];

        /// <summary>
        /// ID of the original response message, present only on follow-up messages
        /// </summary>
        [JsonPropertyName("original_response_message_id")]
        public ulong? OriginalResponseMessageId { get; init; }

        /// <summary>
        /// The user the command was run on, present only on user command interactions
        /// </summary>
        [JsonPropertyName("target_user")]
        public User? TargetUser { get; init; }

        /// <summary>
        /// The ID of the message the command was run on, present only on message command interactions.
        /// The original response message will also have message_reference and referenced_message pointing to this message.
        /// </summary>
        [JsonPropertyName("target_message_id")]
        public ulong? TargetMessageId { get; init; }
    }
}