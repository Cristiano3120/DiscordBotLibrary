namespace DiscordBotLibrary.RequestGuildMembersResources
{

    internal sealed class GuildMembersChunk
    {
        /// <summary>
        /// ID of the guild
        /// </summary>
        [JsonProperty("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong GuildId { get; init; }

        [JsonProperty("members")]
        public GuildMember[] Members { get; init; } = [];

        /// <summary>
        /// Chunk index in the expected chunks for this response (0 <= chunk\_index < chunk\_count)
        /// </summary>
        [JsonProperty("chunk_index")]
        public int ChunkIndex { get; init; }

        /// <summary>
        /// Total number of expected chunks for this response
        /// </summary>
        [JsonProperty("chunk_count")]
        public int ChunkCount { get; init; }

        /// <summary>
        /// When passing an invalid ID to REQUEST_GUILD_MEMBERS, it will be returned here
        /// </summary>
        [JsonProperty("not_found")]
        [JsonConverter(typeof(SnowflakeArrayConverter))]
        public ulong[] NotFound { get; init; } = [];

        /// <summary>
        /// When passing true to REQUEST_GUILD_MEMBERS, presences of the returned members will be here
        /// </summary>
        [JsonProperty("presences")]
        public Presence[]? Presences { get; init; }

        /// <summary>
        /// Nonce used in the Guild Members Request
        /// </summary>
        [JsonProperty("nonce")]
        public string Nonce { get; init; } = string.Empty;
    }
}
