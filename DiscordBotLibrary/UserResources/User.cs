namespace DiscordBotLibrary.UserResources
{
    /// <summary>
    /// Represents a Discord user object.
    /// </summary>
    public sealed record User
    {
        /// <summary>
        /// The user's unique ID.
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// The user's username (not unique across the platform).
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; init; } = string.Empty;

        /// <summary>
        /// The user's Discord tag (discriminator).
        /// </summary>
        [JsonPropertyName("discriminator")]
        public string Discriminator { get; init; } = string.Empty;

        /// <summary>
        /// The user's global display name, if set. For bots, this is the application name.
        /// </summary>
        [JsonPropertyName("global_name")]
        public string? GlobalName { get; init; }

        /// <summary>
        /// The user's avatar hash.
        /// </summary>
        [JsonPropertyName("avatar")]
        public string? Avatar { get; init; }

        /// <summary>
        /// Indicates whether the user belongs to an OAuth2 application.
        /// </summary>
        [JsonPropertyName("bot")]
        public bool? Bot { get; init; }

        /// <summary>
        /// Indicates whether the user is a Discord system user.
        /// </summary>
        [JsonPropertyName("system")]
        public bool? System { get; init; }

        /// <summary>
        /// Indicates whether the user has two-factor authentication enabled.
        /// </summary>
        [JsonPropertyName("mfa_enabled")]
        public bool? MfaEnabled { get; init; }

        /// <summary>
        /// The user's banner hash.
        /// </summary>
        [JsonPropertyName("banner")]
        public string? Banner { get; init; }

        /// <summary>
        /// The user's banner color as an integer representation of a hex color code.
        /// </summary>
        [JsonPropertyName("accent_color")]
        public int? AccentColor { get; init; }

        /// <summary>
        /// The user's chosen language/locale.
        /// </summary>
        [JsonPropertyName("locale")]
        public Language? Locale { get; init; }

        /// <summary>
        /// Indicates whether the user's email address has been verified.
        /// </summary>
        [JsonPropertyName("verified")]
        public bool? Verified { get; init; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; init; }

        /// <summary>
        /// Flags on the user's account.
        /// </summary>
        [JsonPropertyName("flags")]
        public UserFlags? Flags { get; init; }

        /// <summary>
        /// The type of Nitro subscription the user has.
        /// </summary>
        [JsonPropertyName("premium_type")]
        public NitroType? PremiumType { get; init; }

        /// <summary>
        /// The public flags on the user's account.
        /// </summary>
        [JsonPropertyName("public_flags")]
        public UserFlags? PublicFlags { get; init; }

        /// <summary>
        /// Data for the user's avatar decoration.
        /// </summary>
        [JsonPropertyName("avatar_decoration_data")]
        public AvatarDecorationData? AvatarDecorationData { get; init; }
    }

}
