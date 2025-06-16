namespace DiscordBotLibrary.MessageResources
{
    public sealed record MessageCall
    {
        /// <summary>
        /// array of user object ids that participated in the call
        /// </summary>
        [JsonPropertyName("participants")]
        [JsonConverter(typeof(SnowflakeArrayConverter))]
        public ulong[] Participants { get; init; } = [];

        /// <summary>
        /// time when call ended
        /// </summary>
        [JsonPropertyName("ended_timestamp")]
        public DateTimeOffset? EndedTimestamp { get; init; }
    }
}