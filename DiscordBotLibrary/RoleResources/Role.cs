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
        [JsonPropertyName("id")]
        public string Id { get; init; } = default!;

        /// <summary>
        /// Gets the name of the role.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// Gets the role's color as an integer representation of a hexadecimal color code.
        /// </summary>
        [JsonPropertyName("color")]
        public int Color { get; init; }

        /// <summary>
        /// Gets a value indicating whether this role is pinned in the user listing.
        /// </summary>
        [JsonPropertyName("hoist")]
        public bool Hoist { get; init; }

        /// <summary>
        /// Gets the hash of the role's icon, if any.
        /// </summary>
        [JsonPropertyName("icon")]
        public string? Icon { get; init; }

        /// <summary>
        /// Gets the unicode emoji associated with this role, if any.
        /// </summary>
        [JsonPropertyName("unicode_emoji")]
        public string? UnicodeEmoji { get; init; }

        /// <summary>
        /// Gets the position of this role in the role list. 
        /// Roles with the same position are sorted by ID.
        /// </summary>
        [JsonPropertyName("position")]
        public uint Position { get; init; }

        /// <summary>
        /// Gets the permission bit set for this role.
        /// Represented as a string bitmask.
        /// </summary>
        [JsonPropertyName("permissions")]
        public string Permissions { get; init; } = default!;

        /// <summary>
        /// Gets a value indicating whether this role is managed by an integration.
        /// </summary>
        [JsonPropertyName("managed")]
        public bool Managed { get; init; }

        /// <summary>
        /// Gets a value indicating whether this role is mentionable.
        /// </summary>
        [JsonPropertyName("mentionable")]
        public bool Mentionable { get; init; }

        /// <summary>
        /// Gets the tags associated with this role, such as bot ID or integration ID.
        /// </summary>
        [JsonPropertyName("tags")]
        public RoleTags? Tags { get; init; }

        /// <summary>
        /// Gets the combined role flags as a bitfield.
        /// </summary>
        [JsonPropertyName("flags")]
        public RoleFlags Flags { get; init; }
    }

}
