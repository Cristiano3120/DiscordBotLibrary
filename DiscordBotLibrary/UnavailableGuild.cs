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
        [JsonProperty("id")]
        public ulong Id { get; init; }

        /// <summary>
        /// True if the guild is unavailable.
        /// </summary>
        [JsonProperty("unavailable")]
        public bool Unavailable { get; init; }

        public static implicit operator UnavailableGuild(DiscordGuild discordGuild)
            => new()
            {
                Id = discordGuild.Id,
                Unavailable = true
            };
    }
}