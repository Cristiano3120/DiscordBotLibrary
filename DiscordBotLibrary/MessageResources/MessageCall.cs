using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.MessageResources
{
    public sealed record MessageCall
    {
        /// <summary>
        /// array of user object ids that participated in the call
        /// </summary>
        [JsonProperty("participants")]
        [JsonConverter(typeof(SnowflakeArrayConverter))]
        public ulong[] Participants { get; init; } = [];

        /// <summary>
        /// time when call ended
        /// </summary>
        [JsonProperty("ended_timestamp")]
        public DateTimeOffset? EndedTimestamp { get; init; }
    }
}