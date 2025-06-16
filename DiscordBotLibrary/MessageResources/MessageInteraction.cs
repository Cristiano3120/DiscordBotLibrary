namespace DiscordBotLibrary.MessageResources
{
    public class MessageInteraction
    {
        [JsonPropertyName("id")]
        public ulong Id { get; init; }

        [JsonPropertyName("type")]
        public InteractionType Type { get; init; }

        /// <summary>
        /// Name of the application command, including subcommands and subcommand groups
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// User who invoked the interaction
        /// </summary>
        [JsonPropertyName("user")]
        public User User { get; init; } = default!;

        /// <summary>
        /// Partial Member who invoked the interaction in the guild
        /// </summary>
        [JsonPropertyName("member")]
        public GuildMember? Member { get; init; }
    }
}