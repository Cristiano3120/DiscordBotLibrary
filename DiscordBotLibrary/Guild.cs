namespace DiscordBotLibrary
{
    using System.Text.Json.Serialization;
    using DiscordBotLibrary.RoleResources;
    using DiscordBotLibrary.StickerResources;

    public class Guild
    {
        /// <summary>
        /// Guild id
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        /// <summary>
        /// Guild name (2-100 characters, excluding trailing and leading whitespace)
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Icon hash
        /// </summary>
        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        /// <summary>
        /// Icon hash, returned when in the template object
        /// </summary>
        [JsonPropertyName("icon_hash")]
        public string? IconHash { get; set; }

        /// <summary>
        /// Splash hash
        /// </summary>
        [JsonPropertyName("splash")]
        public string? Splash { get; set; }

        /// <summary>
        /// Discovery splash hash; only present for guilds with the "DISCOVERABLE" feature
        /// </summary>
        [JsonPropertyName("discovery_splash")]
        public string? DiscoverySplash { get; set; }

        /// <summary>
        /// <c>True</c> if the user is the owner of the guild
        /// </summary>
        [JsonPropertyName("owner")]
        public bool? Owner { get; set; }

        /// <summary>
        /// The user id of the owner of the guild
        /// </summary>
        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; } = default!;

        /// <summary>
        /// Total permissions for the user in the guild (excludes overwrites and implicit permissions)
        /// </summary>
        [JsonPropertyName("permissions")]
        public string? Permissions { get; set; }

        /// <summary>
        /// The voice region of the guild (deprecated)
        /// </summary>
        [Obsolete("This attribute is no longer used in newer api versions")]
        [JsonPropertyName("region")]
        public string? Region { get; init; }

        /// <summary>
        /// Id of afk channel (not every Guild has one)
        /// </summary>
        [JsonPropertyName("afk_channel_id")]
        public string? AfkChannelId { get; set; }

        /// <summary>
        /// Afk timeout in seconds
        /// </summary>
        [JsonPropertyName("afk_timeout")]
        public int AfkTimeout { get; set; }

        /// <summary>
        /// <c>True</c> if the server widget is enabled
        /// </summary>
        [JsonPropertyName("widget_enabled")]
        public bool? WidgetEnabled { get; set; }

        /// <summary>
        /// The channel id that the widget will generate an invite to, or <c>null</c> if set to no invite
        /// </summary>
        [JsonPropertyName("widget_channel_id")]
        public string? WidgetChannelId { get; set; }

        /// <summary>
        /// The verification level required for the guild
        /// </summary>
        [JsonPropertyName("verification_level")]
        public VerificationLevel VerificationLevel { get; set; }

        /// <summary>
        /// The default message notification level for the guild.
        /// </summary>
        [JsonPropertyName("default_message_notifications")]
        public NotificationLevel DefaultMessageNotifications { get; set; }

        /// <summary>
        /// The explicit content filter level for the guild.
        /// </summary>
        [JsonPropertyName("explicit_content_filter")]
        public ExplicitContentFilterLevel ExplicitContentFilter { get; set; }

        /// <summary>
        /// The roles in the guild.
        /// </summary>
        [JsonPropertyName("roles")]
        public List<Role> Roles { get; set; } = [];

        /// <summary>
        /// Custom emojis of the guild.
        /// </summary>
        [JsonPropertyName("emojis")]
        public List<Emoji> Emojis { get; set; } = [];

        /// <summary>
        /// Enabled guild features.
        /// </summary>
        [JsonPropertyName("features")]
        public List<string> Features { get; set; } = [];

        /// <summary>
        /// The required MFA level for the guild.
        /// </summary>
        [JsonPropertyName("mfa_level")]
        public MfaLevel MfaLevel { get; set; }

        /// <summary>
        /// The application ID of the guild creator if it is bot-created.
        /// </summary>
        [JsonPropertyName("application_id")]
        public string? ApplicationId { get; set; }

        /// <summary>
        /// The ID of the channel where guild notices such as welcome messages and boost events are posted.
        /// </summary>
        [JsonPropertyName("system_channel_id")]
        public string? SystemChannelId { get; set; }

        /// <summary>
        /// System channel flags for the guild.
        /// </summary>
        [JsonPropertyName("system_channel_flags")]
        public SystemChannelFlags SystemChannelFlags { get; set; }

        /// <summary>
        /// The ID of the channel where Community guilds can display rules and/or guidelines.
        /// </summary>
        [JsonPropertyName("rules_channel_id")]
        public string? RulesChannelId { get; set; }

        /// <summary>
        /// The maximum number of presences for the guild. (Always null except for the largest guilds.)
        /// </summary>
        [JsonPropertyName("max_presences")]
        public int? MaxPresences { get; set; }

        /// <summary>
        /// The maximum number of members for the guild.
        /// </summary>
        [JsonPropertyName("max_members")]
        public int? MaxMembers { get; set; }

        /// <summary>
        /// The vanity URL code for the guild.
        /// </summary>
        [JsonPropertyName("vanity_url_code")]
        public string? VanityUrlCode { get; set; }

        /// <summary>
        /// The description of the guild.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The banner hash for the guild.
        /// </summary>
        [JsonPropertyName("banner")]
        public string? Banner { get; set; }

        /// <summary>
        /// The premium tier (Server Boost level) of the guild.
        /// </summary>
        [JsonPropertyName("premium_tier")]
        public PremiumTier PremiumTier { get; set; }

        /// <summary>
        /// The number of boosts this guild currently has.
        /// </summary>
        [JsonPropertyName("premium_subscription_count")]
        public int? PremiumSubscriptionCount { get; set; }

        /// <summary>
        /// The preferred locale of a Community guild, used in server discovery and notices from Discord, and sent in interactions. Defaults to "en-US".
        /// </summary>
        [JsonPropertyName("preferred_locale")]
        public string? PreferredLocale { get; set; }

        /// <summary>
        /// The ID of the channel where admins and moderators of Community guilds receive notices from Discord.
        /// </summary>
        [JsonPropertyName("public_updates_channel_id")]
        public string? PublicUpdatesChannelId { get; set; }

        /// <summary>
        /// The maximum number of users in a video channel.
        /// </summary>
        [JsonPropertyName("max_video_channel_users")]
        public int? MaxVideoChannelUsers { get; set; }

        /// <summary>
        /// The maximum number of users in a stage video channel.
        /// </summary>
        [JsonPropertyName("max_stage_video_channel_users")]
        public int? MaxStageVideoChannelUsers { get; set; }

        /// <summary>
        /// Approximate number of members in this guild, returned when with_counts is true.
        /// </summary>
        [JsonPropertyName("approximate_member_count")]
        public int? ApproximateMemberCount { get; set; }

        /// <summary>
        /// Approximate number of non-offline members in this guild, returned when with_counts is true.
        /// </summary>
        [JsonPropertyName("approximate_presence_count")]
        public int? ApproximatePresenceCount { get; set; }

        /// <summary>
        /// The welcome screen of a Community guild, shown to new members. Returned in an Invite's guild object.
        /// </summary>
        [JsonPropertyName("welcome_screen")]
        public WelcomeScreen? WelcomeScreen { get; set; }

        /// <summary>
        /// The guild NSFW level.
        /// </summary>
        [JsonPropertyName("nsfw_level")]
        public NsfwLevel NsfwLevel { get; set; }

        /// <summary>
        /// Custom guild stickers.
        /// </summary>
        [JsonPropertyName("stickers")]
        public List<Sticker>? Stickers { get; set; }

        /// <summary>
        /// Indicates whether the guild has the boost progress bar enabled.
        /// </summary>
        [JsonPropertyName("premium_progress_bar_enabled")]
        public bool PremiumProgressBarEnabled { get; set; }

        /// <summary>
        /// The ID of the channel where admins and moderators of Community guilds receive safety alerts from Discord.
        /// </summary>
        [JsonPropertyName("safety_alerts_channel_id")]
        public string? SafetyAlertsChannelId { get; set; }

        /// <summary>
        /// The incidents data for this guild.
        /// </summary>
        [JsonPropertyName("incidents_data")]
        public IncidentsData? IncidentsData { get; set; }
    }
}
