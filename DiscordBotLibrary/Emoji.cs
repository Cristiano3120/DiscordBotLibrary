using DiscordBotLibrary.RoleResources;
using DiscordBotLibrary.UserResources;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a Discord emoji
    /// </summary>
    public record Emoji
    {
        /// <summary>
        /// The id of the emoji
        /// TYPE: Snowflake
        /// </summary>
        public string? Id { get; init; }

        /// <summary>
        /// The name of the emoji
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Roles allowed to use this emoji
        /// </summary>
        public Role[]? Roles { get; init; }

        /// <summary>
        /// The user that created this emoji
        /// </summary>
        public User? User { get; init; }

        /// <summary>
        /// Whether this emoji must be wrapped in colons
        /// </summary>
        public bool? RequireColons { get; init; }

        /// <summary>
        /// Whether this emoji is managed
        /// </summary>
        public bool? Managed { get; init; }

        /// <summary>
        /// Whether this emoji is animated
        /// </summary>
        public bool? Animated { get; init; }

        /// <summary>
        /// Whether this emoji can be used, may be false due to loss of Server Boosts
        /// </summary>
        public bool? Available { get; init; }
    }
}
