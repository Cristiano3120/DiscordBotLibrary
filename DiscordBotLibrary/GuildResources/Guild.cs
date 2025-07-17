using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.GuildResources
{
    public class Guild
    {
        /// <summary>
        /// Guild id
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; }

        /// <summary>
        /// Guild name (2-100 characters, excluding trailing and leading whitespace)
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// Icon hash
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; init; }

        /// <summary>
        /// Icon hash, returned when in the template object
        /// </summary>
        [JsonProperty("icon_hash")]
        public string? IconHash { get; init; }

        /// <summary>
        /// Splash hash
        /// </summary>
        [JsonProperty("splash")]
        public string? Splash { get; init; }

        /// <summary>
        /// Discovery splash hash; only present for guilds with the "DISCOVERABLE" feature
        /// </summary>
        [JsonProperty("discovery_splash")]
        public string? DiscoverySplash { get; init; }

        /// <summary>
        /// <c>True</c> if the user is the owner of the guild
        /// </summary>
        [JsonProperty("owner")]
        public bool? Owner { get; init; }

        /// <summary>
        /// The user id of the owner of the guild
        /// </summary>
        [JsonProperty("owner_id")]
        public ulong OwnerId { get; init; } = default!;

        /// <summary>
        /// Total permissions for the user in the guild (excludes overwrites and implicit permissions)
        /// </summary>
        [JsonProperty("permissions")]
        public string? Permissions { get; init; }

        /// <summary>
        /// The voice region of the guild (deprecated)
        /// </summary>
        [Obsolete("This attribute is no longer used in newer api versions")]
        [JsonProperty("region")]
        public string? Region { get; init; }

        /// <summary>
        /// Id of afk channel (not every Guild has one)
        /// </summary>
        [JsonProperty("afk_channel_id")]
        public string? AfkChannelId { get; init; }

        /// <summary>
        /// Afk timeout in seconds
        /// </summary>
        [JsonProperty("afk_timeout")]
        public int AfkTimeout { get; init; }

        /// <summary>
        /// <c>True</c> if the server widget is enabled
        /// </summary>
        [JsonProperty("widget_enabled")]
        public bool? WidgetEnabled { get; init; }

        /// <summary>
        /// The channel id that the widget will generate an invite to, or <c>null</c> if set to no invite
        /// </summary>
        [JsonProperty("widget_channel_id")]
        public string? WidgetChannelId { get; init; }

        /// <summary>
        /// The verification level required for the guild
        /// </summary>
        [JsonProperty("verification_level")]
        public VerificationLevel VerificationLevel { get; init; }

        /// <summary>
        /// The default message notification level for the guild.
        /// </summary>
        [JsonProperty("default_message_notifications")]
        public NotificationLevel DefaultMessageNotifications { get; init; }

        /// <summary>
        /// The explicit content filter level for the guild.
        /// </summary>
        [JsonProperty("explicit_content_filter")]
        public ExplicitContentFilterLevel ExplicitContentFilter { get; init; }

        /// <summary>
        /// The roles in the guild.
        /// </summary>
        [JsonProperty("roles")]
        public List<Role> Roles { get; init; } = [];

        /// <summary>
        /// Custom emojis of the guild.
        /// </summary>
        [JsonProperty("emojis")]
        public List<Emoji> Emojis { get; init; } = [];

        /// <summary>
        /// Enabled guild features.
        /// </summary>
        public GuildFeature[] Features { get; init; } = [];

        /// <summary>
        /// The required MFA level for the guild.
        /// </summary>
        [JsonProperty("mfa_level")]
        public MfaLevel MfaLevel { get; init; }

        /// <summary>
        /// The application ID of the guild creator if it is bot-created.
        /// </summary>
        [JsonProperty("application_id")]
        public string? ApplicationId { get; init; }

        /// <summary>
        /// The ID of the channel where guild notices such as welcome messages and boost events are posted.
        /// </summary>
        [JsonProperty("system_channel_id")]
        public string? SystemChannelId { get; init; }

        /// <summary>
        /// System channel flags for the guild.
        /// </summary>
        [JsonProperty("system_channel_flags")]
        public SystemChannelFlags SystemChannelFlags { get; init; }

        /// <summary>
        /// The ID of the channel where Community guilds can display rules and/or guidelines.
        /// </summary>
        [JsonProperty("rules_channel_id")]
        public string? RulesChannelId { get; init; }

        /// <summary>
        /// The maximum number of presences for the guild. (Always null except for the largest guilds.)
        /// </summary>
        [JsonProperty("max_presences")]
        public int? MaxPresences { get; init; }

        /// <summary>
        /// The maximum number of members for the guild.
        /// </summary>
        [JsonProperty("max_members")]
        public int? MaxMembers { get; init; }

        /// <summary>
        /// The vanity URL code for the guild.
        /// </summary>
        [JsonProperty("vanity_url_code")]
        public string? VanityUrlCode { get; init; }

        /// <summary>
        /// The description of the guild.
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; init; }

        /// <summary>
        /// The banner hash for the guild.
        /// </summary>
        [JsonProperty("banner")]
        public string? Banner { get; init; }

        /// <summary>
        /// The premium tier (Server Boost level) of the guild.
        /// </summary>
        [JsonProperty("premium_tier")]
        public PremiumTier PremiumTier { get; init; }

        /// <summary>
        /// The number of boosts this guild currently has.
        /// </summary>
        [JsonProperty("premium_subscription_count")]
        public int? PremiumSubscriptionCount { get; init; }

        /// <summary>
        /// The preferred locale of a Community guild, used in server discovery and notices from Discord, and sent in interactions. Defaults to "en-US".
        /// </summary>
        [JsonProperty("preferred_locale")]
        public Language? PreferredLocale { get; init; }

        /// <summary>
        /// The ID of the channel where admins and moderators of Community guilds receive notices from Discord.
        /// </summary>
        [JsonProperty("public_updates_channel_id")]
        public string? PublicUpdatesChannelId { get; init; }

        /// <summary>
        /// The maximum number of users in a video channel.
        /// </summary>
        [JsonProperty("max_video_channel_users")]
        public int? MaxVideoChannelUsers { get; init; }

        /// <summary>
        /// The maximum number of users in a stage video channel.
        /// </summary>
        [JsonProperty("max_stage_video_channel_users")]
        public int? MaxStageVideoChannelUsers { get; init; }

        /// <summary>
        /// Approximate number of members in this guild, returned when with_counts is true.
        /// </summary>
        [JsonProperty("approximate_member_count")]
        public int? ApproximateMemberCount { get; init; }

        /// <summary>
        /// Approximate number of non-offline members in this guild, returned when with_counts is true.
        /// </summary>
        [JsonProperty("approximate_presence_count")]
        public int? ApproximatePresenceCount { get; init; }

        /// <summary>
        /// The welcome screen of a Community guild, shown to new members. Returned in an Invite's guild object.
        /// </summary>
        [JsonProperty("welcome_screen")]
        public WelcomeScreen? WelcomeScreen { get; init; }

        /// <summary>
        /// The guild NSFW level.
        /// </summary>
        [JsonProperty("nsfw_level")]
        public NsfwLevel NsfwLevel { get; init; }

        /// <summary>
        /// Custom guild stickers.
        /// </summary>
        [JsonProperty("stickers")]
        public List<Sticker>? Stickers { get; init; }

        /// <summary>
        /// Indicates whether the guild has the boost progress bar enabled.
        /// </summary>
        [JsonProperty("premium_progress_bar_enabled")]
        public bool PremiumProgressBarEnabled { get; init; }

        /// <summary>
        /// The ID of the channel where admins and moderators of Community guilds receive safety alerts from Discord.
        /// </summary>
        [JsonProperty("safety_alerts_channel_id")]
        public string? SafetyAlertsChannelId { get; init; }

        /// <summary>
        /// The incidents data for this guild.
        /// </summary>
        [JsonProperty("incidents_data")]
        public IncidentsData? IncidentsData { get; init; }
    }
}
