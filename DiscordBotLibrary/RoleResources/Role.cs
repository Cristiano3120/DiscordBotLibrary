using DiscordBotLibrary.Json.Converters.BitsetConverters;

namespace DiscordBotLibrary.RoleResources
{
    /// <summary>
    /// Represents a role within a Discord guild.
    /// </summary>
    public record Role
    {
        /// <summary>
        /// Gets the unique identifier for the role.
        /// </summary>
        [JsonProperty("id")]
        public ulong Id { get; init; } = default!;

        /// <summary>
        /// Gets the name of the role.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// Gets the role's color as an integer representation of a hexadecimal color code.
        /// </summary>
        [JsonProperty("color")]
        public int Color { get; init; }

        /// <summary>
        /// Gets a value indicating whether this role is pinned in the user listing.
        /// </summary>
        [JsonProperty("hoist")]
        public bool Hoist { get; init; }

        /// <summary>
        /// Gets the hash of the role's icon, if any.
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; init; }

        /// <summary>
        /// Gets the unicode emoji associated with this role, if any.
        /// </summary>
        [JsonProperty("unicode_emoji")]
        public string? UnicodeEmoji { get; init; }

        /// <summary>
        /// Gets the position of this role in the role list. 
        /// Roles with the same position are sorted by ID.
        /// </summary>
        [JsonProperty("position")]
        public uint Position { get; init; }

        /// <summary>
        /// Gets the permission bit set for this role.
        /// </summary>
        [JsonConverter(typeof(PermissionsConverter))]
        public DiscordPermissions Permissions { get; init; } = default!;

        /// <summary>
        /// Gets a value indicating whether this role is managed by an integration.
        /// </summary>
        [JsonProperty("managed")]
        public bool Managed { get; init; }

        /// <summary>
        /// Gets a value indicating whether this role is mentionable.
        /// </summary>
        [JsonProperty("mentionable")]
        public bool Mentionable { get; init; }

        /// <summary>
        /// Gets the tags associated with this role, such as bot ID or integration ID.
        /// </summary>
        [JsonProperty("tags")]
        public RoleTags? Tags { get; init; }

        /// <summary>
        /// Gets the combined role flags as a bitfield.
        /// </summary>
        [JsonProperty("flags")]
        public RoleFlags Flags { get; init; }
    }

}
