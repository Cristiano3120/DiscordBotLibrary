using DiscordBotLibrary.Json.Converters.BitsetConverters;

namespace DiscordBotLibrary.GuildMemberResources
{
    /// <summary>
    /// Represents a guild member in a Discord server.
    /// </summary>
    public sealed record GuildMember
    {
        /// <summary>
        /// The user this guild member represents (optional).
        /// </summary>
        [JsonProperty("user")]
        public User? User { get; init; }

        /// <summary>
        /// This user's guild nickname (optional).
        /// </summary>
        [JsonProperty("nick")]
        public string? Nickname { get; init; }

        /// <summary>
        /// The member's guild avatar hash (optional).
        /// </summary>
        [JsonProperty("avatar")]
        public string? Avatar { get; init; }

        /// <summary>
        /// The member's guild banner hash (optional).
        /// </summary>
        [JsonProperty("banner")]
        public string? Banner { get; init; }

        /// <summary>
        /// Array of role object IDs (snowflakes).
        /// </summary>
        [JsonProperty("roles")]
        public ulong[] Roles { get; init; } = [];

        /// <summary>
        /// When the user joined the guild.
        /// </summary>
        [JsonProperty("joined_at")]
        public DateTime JoinedAt { get; init; }

        /// <summary>
        /// When the user started boosting the guild (optional).
        /// </summary>
        [JsonProperty("premium_since")]
        public DateTime? PremiumSince { get; init; }

        /// <summary>
        /// Whether the user is deafened in voice channels.
        /// </summary>
        [JsonProperty("deaf")]
        public bool Deaf { get; init; }

        /// <summary>
        /// Whether the user is muted in voice channels.
        /// </summary>
        [JsonProperty("mute")]
        public bool Mute { get; init; }

        /// <summary>
        /// Guild member flags represented as a bit set. Defaults to 0.
        /// </summary>
        [JsonProperty("flags")]
        public GuildMemberFlags Flags { get; init; }

        /// <summary>
        /// Whether the user has not yet passed the guild's Membership Screening requirements (optional).
        /// </summary>
        [JsonProperty("pending")]
        public bool? Pending { get; init; }

        /// <summary>
        /// Total permissions of the member in the channel, including overwrites.
        /// Returned only in the interaction object (optional).
        /// </summary>
        [JsonConverter(typeof(PermissionsConverter))]
        public DiscordPermissions? Permissions { get; init; }

        /// <summary>
        /// When the user's timeout will expire and the user will be able to communicate again.
        /// Null or time in the past if the user is not timed out (optional).
        /// </summary>
        [JsonProperty("communication_disabled_until")]
        public DateTime? CommunicationDisabledUntil { get; init; }

        /// <summary>
        /// Data for the member's guild avatar decoration (optional).
        /// </summary>
        [JsonProperty("avatar_decoration_data")]
        public AvatarDecorationData? AvatarDecorationData { get; init; }

        /// <summary>
        /// The member's presence in the guild, if requested.
        /// </summary>
        [JsonIgnore]
        public Presence? Presence { get; private set; } = null;

        internal void SetPresence(Presence? presence)
        {
            Presence = presence;
        }
    }
}