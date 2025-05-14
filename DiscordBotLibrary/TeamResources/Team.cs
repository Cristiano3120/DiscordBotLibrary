namespace DiscordBotLibrary.TeamResources
{
    /// <summary>
    /// Represents a team object in Discord.
    /// </summary>
    public sealed record Team
    {
        /// <summary>
        /// Gets the hash of the image for the team's icon, if available.
        /// </summary>
        [JsonPropertyName("icon")]
        public string? Icon { get; init; }

        /// <summary>
        /// Gets the unique ID of the team.
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; } = default!;

        /// <summary>
        /// Gets the list of members associated with the team.
        /// </summary>
        [JsonPropertyName("members")]
        public TeamMember[] Members { get; init; } = [];

        /// <summary>
        /// Gets the name of the team.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// Gets the user ID of the current team owner.
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("owner_user_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong OwnerUserId { get; init; } = default!;
    }

}