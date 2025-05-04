namespace DiscordBotLibrary.GuildMemberResources
{
    /// <summary>
    /// Flags that describe the state or privileges of a guild member.
    /// </summary>
    [Flags]
    public enum GuildMemberFlags
    {
        /// <summary>
        /// No flags are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Member has left and rejoined the guild.
        /// </summary>
        DidRejoin = 1 << 0,

        /// <summary>
        /// Member has completed onboarding.
        /// </summary>
        CompletedOnboarding = 1 << 1,

        /// <summary>
        /// Member is exempt from guild verification requirements.
        /// </summary>
        BypassesVerification = 1 << 2,

        /// <summary>
        /// Member has started onboarding.
        /// </summary>
        StartedOnboarding = 1 << 3,

        /// <summary>
        /// Member is a guest and can only access the voice channel they were invited to.
        /// </summary>
        IsGuest = 1 << 4,

        /// <summary>
        /// Member has started Server Guide new member actions.
        /// </summary>
        StartedHomeActions = 1 << 5,

        /// <summary>
        /// Member has completed Server Guide new member actions.
        /// </summary>
        CompletedHomeActions = 1 << 6,

        /// <summary>
        /// Member's username, display name, or nickname is blocked by AutoMod.
        /// </summary>
        AutomodQuarantinedUsername = 1 << 7,

        /// <summary>
        /// Member has dismissed the DM settings upsell.
        /// </summary>
        DmSettingsUpsellAcknowledged = 1 << 9
    }
}
