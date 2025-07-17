using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a Discord emoji
    /// </summary>
    public sealed record Emoji
    {
        /// <summary>
        /// The id of the emoji
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? Id { get; init; }

        /// <summary>
        /// The name of the emoji
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; init; }

        /// <summary>
        /// Roles allowed to use this emoji
        /// </summary>
        [JsonProperty("roles")]
        public Role[]? Roles { get; init; }

        /// <summary>
        /// The user that created this emoji
        /// </summary>
        [JsonProperty("user")]
        public User? User { get; init; }

        /// <summary>
        /// Whether this emoji must be wrapped in colons
        /// </summary>
        [JsonProperty("require_colons")]
        public bool? RequireColons { get; init; }

        /// <summary>
        /// Whether this emoji is managed
        /// </summary>
        [JsonProperty("managed")]
        public bool? Managed { get; init; }

        /// <summary>
        /// Whether this emoji is animated
        /// </summary>
        [JsonProperty("animated")]
        public bool? Animated { get; init; }

        /// <summary>
        /// Whether this emoji can be used, may be false due to loss of Server Boosts
        /// </summary>
        [JsonProperty("available")]
        public bool? Available { get; init; }
    }
}
