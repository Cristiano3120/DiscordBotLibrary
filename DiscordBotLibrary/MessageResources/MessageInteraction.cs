namespace DiscordBotLibrary.MessageResources
{
    public class MessageInteraction
    {
        [JsonProperty("id")]
        public ulong Id { get; init; }

        [JsonProperty("type")]
        public InteractionType Type { get; init; }

        /// <summary>
        /// Name of the application command, including subcommands and subcommand groups
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// User who invoked the interaction
        /// </summary>
        [JsonProperty("user")]
        public User User { get; init; } = default!;

        /// <summary>
        /// Partial Member who invoked the interaction in the guild
        /// </summary>
        [JsonProperty("member")]
        public GuildMember? Member { get; init; }
    }
}