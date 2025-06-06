﻿namespace DiscordBotLibrary.ApplicationResources
{
    /// <summary>
    /// Represents the settings for the application's default in-app authorization link.
    /// </summary>
    public readonly struct InstallParams
    {
        /// <summary>
        /// Scopes to add the application to the server with.
        /// </summary>
        [JsonPropertyName("scopes")]
        public OAuth2Scope[] Scopes { get; init; }

        /// <summary>
        /// Permissions to request for the bot role.
        /// </summary>
        [JsonPropertyName("permissions")]
        public DiscordPermission Permissions { get; init; }
    }
}