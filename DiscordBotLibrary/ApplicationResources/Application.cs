using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.ApplicationResources
{
    /// <summary>
    /// Represents a application object in Discord.
    /// </summary>
    public sealed record Application
    {
        /// <summary>
        /// Gets the ID of the application.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = default!;

        /// <summary>
        /// Gets the icon hash of the application.
        /// </summary>
        [JsonProperty("icon")]
        public string? Icon { get; init; }

        /// <summary>
        /// Gets the description of the application.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; init; } = default!;

        /// <summary>
        /// Gets an array of RPC origin URLs, if RPC is enabled.
        /// </summary>
        [JsonProperty("rpc_origins")]
        public string[] RpcOrigins { get; init; } = [];

        /// <summary>
        /// Gets a value indicating whether the bot can be added to guilds by anyone.
        /// If <c>false</c> only the bot owner can add this bot to a guild.
        /// </summary>
        [JsonProperty("bot_public")]
        public bool BotPublic { get; init; }

        /// <summary>
        /// Gets a value indicating whether the bot will only join upon completion of the full OAuth2 code grant flow.
        /// </summary>
        [JsonProperty("bot_require_code_grant")]
        public bool BotRequireCodeGrant { get; init; }

        /// <summary>
        /// Gets a partial user object for the bot user associated with the application.
        /// </summary>
        [JsonProperty("bot")]
        public User? Bot { get; init; }

        /// <summary>
        /// Gets the URL of the application's Terms of Service.
        /// </summary>
        [JsonProperty("terms_of_service_url")]
        public string? TermsOfServiceUrl { get; init; }

        /// <summary>
        /// Gets the URL of the application's Privacy Policy.
        /// </summary>
        [JsonProperty("privacy_policy_url")]
        public string? PrivacyPolicyUrl { get; init; }

        /// <summary>
        /// Gets a partial user object for the owner of the application.
        /// </summary>
        [JsonProperty("owner")]
        public User? Owner { get; init; }

        /// <summary>
        /// Gets the verification key used for interactions and the GameSDK's GetTicket.
        /// </summary>
        [JsonProperty("verify_key")]
        public string VerifyKey { get; init; } = default!;

        /// <summary>
        /// Gets the team that the application belongs to, if applicable.
        /// </summary>
        [JsonProperty("team")]
        public Team? Team { get; init; }

        /// <summary>
        /// Gets the guild associated with the application, if applicable.
        /// </summary>
        [JsonProperty("guild_id")]
        public string? GuildId { get; init; }

        /// <summary>
        /// Gets a partial object of the associated guild, if applicable.
        /// </summary>
        [JsonProperty("guild")]
        public Guild? Guild { get; init; }

        /// <summary>
        /// Gets the ID of the primary SKU for the application, if applicable.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("primary_sku_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? PrimarySkuId { get; init; }

        /// <summary>
        /// Gets the URL slug that links to the store page of the application, if applicable.
        /// </summary>
        [JsonProperty("slug")]
        public string? Slug { get; init; }

        /// <summary>
        /// Gets the default rich presence invite cover image hash of the application.
        /// </summary>
        [JsonProperty("cover_image")]
        public string? CoverImage { get; init; }

        /// <summary>
        /// Gets the public flags associated with the application.
        /// </summary>
        [JsonProperty("flags")]
        public ApplicationFlags? Flags { get; init; }

        /// <summary>
        /// Gets the approximate number of guilds the application has been added to.
        /// </summary>
        [JsonProperty("approximate_guild_count")]
        public int? ApproximateGuildCount { get; init; }

        /// <summary>
        /// Gets the approximate number of users who have installed the application.
        /// </summary>
        [JsonProperty("approximate_user_install_count")]
        public int? ApproximateUserInstallCount { get; init; }

        /// <summary>
        /// Gets the list of redirect URIs for the application.
        /// </summary>
        [JsonProperty("redirect_uris")]
        public List<string>? RedirectUris { get; init; }

        /// <summary>
        /// Gets the interactions endpoint URL for the application.
        /// </summary>
        [JsonProperty("interactions_endpoint_url")]
        public string? InteractionsEndpointUrl { get; init; }

        /// <summary>
        /// Gets the role connections verification URL for the application.
        /// </summary>
        [JsonProperty("role_connections_verification_url")]
        public string? RoleConnectionsVerificationUrl { get; init; }

        /// <summary>
        /// Gets the event webhooks URL for the application to receive webhook events.
        /// </summary>
        [JsonProperty("event_webhooks_url")]
        public string? EventWebhooksUrl { get; init; }

        /// <summary>
        /// Gets the status of the application's event webhooks.
        /// </summary>
        [JsonProperty("event_webhooks_status")]
        public EventWebhookStatus EventWebhooksStatus { get; init; }

        /// <summary>
        /// Gets the list of event types the application subscribes to for webhooks.
        /// </summary>
        [JsonProperty("event_webhooks_types")]
        public string[]? EventWebhooksTypes { get; init; }

        /// <summary>
        /// Gets the list of tags describing the content and functionality of the application.
        /// </summary>
        [JsonProperty("tags")]
        public string[]? Tags { get; init; }

        /// <summary>
        /// Gets the settings for the application's default in-app authorization link.
        /// </summary>
        [JsonProperty("install_params")]
        public InstallParams? InstallParams { get; init; }

        /// <summary>
        /// Gets the default scopes and permissions for each supported installation context.
        /// </summary>
        [JsonProperty("integration_types_config")]
        public Dictionary<string, IntegrationTypeConfiguration>? IntegrationTypesConfig { get; init; }

        /// <summary>
        /// Gets the custom installation URL for the application, if enabled.
        /// </summary>
        [JsonProperty("custom_install_url")]
        public string? CustomInstallUrl { get; init; }
    }
}