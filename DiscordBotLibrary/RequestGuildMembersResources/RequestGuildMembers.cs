using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.RequestGuildMembersResources
{
    internal sealed record RequestGuildMembers
    {
        /// <summary>
        /// ID of the guild to get members for
        /// </summary>
        [JsonProperty("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong GuildId { get; init; }

        /// <summary>
        /// String that username starts with, or an empty string to return all members
        /// </summary>
        [JsonProperty("query")]
        public string? Query { get; init; } = string.Empty;

        /// <summary>
        /// Maximum number of members to send matching the query;
        /// a limit of 0 can be used with an empty string query to return all members
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; init; }

        /// <summary>
        /// Used to specify if we want the presences of the matched members
        /// </summary>
        [JsonProperty("presences")]
        public bool Presences { get; init; }

        /// <summary>
        /// Used to specify which users you wish to fetch
        /// </summary>
        [JsonProperty("user_ids")]
        [JsonConverter(typeof(SnowflakeArrayConverter))]
        public ulong[]? UserIds { get; init; } = [];

        /// <summary>
        /// Nonce to identify the Guild Members Chunk response
        /// </summary>
        public string Nonce { get; private set; } = Guid.NewGuid().ToString("N");
    }
}
