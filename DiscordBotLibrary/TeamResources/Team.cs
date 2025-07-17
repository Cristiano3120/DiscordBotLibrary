using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

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
        [JsonProperty("icon")]
        public string? Icon { get; init; }

        /// <summary>
        /// Gets the unique ID of the team.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; } = default!;

        /// <summary>
        /// Gets the list of members associated with the team.
        /// </summary>
        [JsonProperty("members")]
        public TeamMember[] Members { get; init; } = [];

        /// <summary>
        /// Gets the name of the team.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// Gets the user ID of the current team owner.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("owner_user_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong OwnerUserId { get; init; } = default!;
    }

}