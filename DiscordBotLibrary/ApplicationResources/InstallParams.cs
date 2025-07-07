namespace DiscordBotLibrary.ApplicationResources
{
    /// <summary>
    /// Represents the settings for the application's default in-app authorization link.
    /// </summary>
    public readonly struct InstallParams
    {
        /// <summary>
        /// Scopes to add the application to the server with.
        /// </summary>
        public OAuth2Scope[] Scopes { get; init; }

        /// <summary>
        /// Permissions to request for the bot role.
        /// </summary>
        public DiscordPermissions Permissions { get; init; }
    }
}