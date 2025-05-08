namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a guild that is currently unavailable.
    /// </summary>
    public readonly struct UnavailableGuild
    {
        /// <summary>
        /// TYPE: Snowflake
        /// ID of the unavailable guild.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; init; }

        /// <summary>
        /// True if the guild is unavailable.
        /// </summary>
        [JsonPropertyName("unavailable")]
        public bool Unavailable { get; init; }
    }
}