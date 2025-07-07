namespace DiscordBotLibrary.MessageResources
{
    public record MessageReference
    {
        /// <summary>
        /// *If type is unset, DEFAULT can be assumed in order to match the behavior before message reference had types. 
        /// In future API versions this will become a required field.
        /// </summary>
        [JsonProperty("type")]
        public ReferenceType? ReferenceType { get; init; }

        [JsonProperty("message_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? MessageId { get; init; }

        /// <summary>
        /// ** channel_id is optional when creating a reply, 
        /// but will always be present when receiving an event/response that includes this data model. 
        /// Required for forwards.
        /// </summary>
        [JsonProperty("channel_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ChannelId { get; init; }

        [JsonProperty("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; init; }

        [JsonProperty("fail_if_not_exists")]
        public bool? FailIfNotExists { get; init; }
    }
}