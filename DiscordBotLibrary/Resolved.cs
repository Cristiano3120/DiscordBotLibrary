namespace DiscordBotLibrary
{
    public sealed record Resolved
    {
        /// <summary>
        /// IDs and User objects
        /// </summary>
        [JsonPropertyName("users")]
        [JsonConverter(typeof(SnowflakeDictConverter<ulong, User>))]
        public Dictionary<ulong, User>? Users { get; init; }

        /// <summary>
        /// IDs and partial Member objects
        /// </summary>
        [JsonPropertyName("members")]
        [JsonConverter(typeof(SnowflakeDictConverter<ulong, GuildMember>))]
        public Dictionary<ulong, GuildMember>? Members { get; init; }

        /// <summary>
        /// IDs and Role objects
        /// </summary>
        [JsonPropertyName("roles")]
        [JsonConverter(typeof(SnowflakeDictConverter<ulong, Role>))]
        public Dictionary<ulong, Role>? Roles { get; init; }

        /// <summary>
        /// IDs and partial Channel objects
        /// </summary>
        [JsonPropertyName("channels")]
        [JsonConverter(typeof(SnowflakeDictConverter<ulong, Channel>))]
        public Dictionary<ulong, Channel>? Channels { get; init; }

        /// <summary>
        /// IDs and partial Message objects
        /// </summary>
        [JsonPropertyName("messages")]
        [JsonConverter(typeof(SnowflakeDictConverter<ulong, Message>))]
        public Dictionary<ulong, Message>? Messages { get; init; }

        /// <summary>
        /// IDs and attachment objects
        /// </summary>
        [JsonPropertyName("attachments")]
        [JsonConverter(typeof(SnowflakeDictConverter<ulong, MessageAttachment>))]
        public Dictionary<ulong, MessageAttachment>? Attachments { get; init; }
    }
}