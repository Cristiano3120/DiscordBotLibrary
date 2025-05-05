namespace DiscordBotLibrary.GuildMemberResources
{
    /// <summary>
    /// Represents a guild member in a Discord server.
    /// </summary>
    public record GuildMember
    {
        /// <summary>
        /// The user this guild member represents (optional).
        /// </summary>
        [JsonPropertyName("user")]
        public User? User { get; init; }

        /// <summary>
        /// This user's guild nickname (optional).
        /// </summary>
        [JsonPropertyName("nick")]
        public string? Nickname { get; init; }

        /// <summary>
        /// The member's guild avatar hash (optional).
        /// </summary>
        [JsonPropertyName("avatar")]
        public string? Avatar { get; init; }

        /// <summary>
        /// The member's guild banner hash (optional).
        /// </summary>
        [JsonPropertyName("banner")]
        public string? Banner { get; init; }

        /// <summary>
        /// Array of role object IDs (snowflakes).
        /// </summary>
        [JsonPropertyName("roles")]
        public string[] Roles { get; init; } = [];

        /// <summary>
        /// When the user joined the guild.
        /// </summary>
        [JsonPropertyName("joined_at")]
        public DateTime JoinedAt { get; init; }

        /// <summary>
        /// When the user started boosting the guild (optional).
        /// </summary>
        [JsonPropertyName("premium_since")]
        public DateTime? PremiumSince { get; init; }

        /// <summary>
        /// Whether the user is deafened in voice channels.
        /// </summary>
        [JsonPropertyName("deaf")]
        public bool Deaf { get; init; }

        /// <summary>
        /// Whether the user is muted in voice channels.
        /// </summary>
        [JsonPropertyName("mute")]
        public bool Mute { get; init; }

        /// <summary>
        /// Guild member flags represented as a bit set. Defaults to 0.
        /// </summary>
        [JsonPropertyName("flags")]
        public GuildMemberFlags Flags { get; init; }

        /// <summary>
        /// Whether the user has not yet passed the guild's Membership Screening requirements (optional).
        /// </summary>
        [JsonPropertyName("pending")]
        public bool? Pending { get; init; }

        /// <summary>
        /// Total permissions of the member in the channel, including overwrites.
        /// Returned only in the interaction object (optional).
        /// </summary>
        [JsonPropertyName("permissions")]
        public string? Permissions { get; init; }

        /// <summary>
        /// When the user's timeout will expire and the user will be able to communicate again.
        /// Null or time in the past if the user is not timed out (optional).
        /// </summary>
        [JsonPropertyName("communication_disabled_until")]
        public DateTime? CommunicationDisabledUntil { get; init; }

        /// <summary>
        /// Data for the member's guild avatar decoration (optional).
        /// </summary>
        [JsonPropertyName("avatar_decoration_data")]
        public AvatarDecorationData? AvatarDecorationData { get; init; }
    }

}