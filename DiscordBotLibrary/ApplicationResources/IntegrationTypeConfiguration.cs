namespace DiscordBotLibrary.ApplicationResources
{
    /// <summary>
    /// Represents the configuration for a specific integration type.
    /// </summary>
    public readonly struct IntegrationTypeConfiguration
    {
        /// <summary>
        /// OAuth2 install parameters for this integration type.
        /// </summary>
        public InstallParams? OAuth2InstallParams { get; init; }
    }
}