namespace DiscordBotLibrary.UserResources
{
    /// <summary>
    /// Flags representing various properties of a Discord user account.
    /// </summary>
    [Flags]
    public enum UserFlags
    {
        /// <summary>
        /// No flags are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Discord Employee.
        /// </summary>
        Staff = 1 << 0,

        /// <summary>
        /// Partnered Server Owner.
        /// </summary>
        Partner = 1 << 1,

        /// <summary>
        /// HypeSquad Events Member.
        /// </summary>
        HypeSquad = 1 << 2,

        /// <summary>
        /// Bug Hunter Level 1.
        /// </summary>
        BugHunterLevel1 = 1 << 3,

        /// <summary>
        /// House Bravery Member.
        /// </summary>
        HypeSquadOnlineHouse1 = 1 << 6,

        /// <summary>
        /// House Brilliance Member.
        /// </summary>
        HypeSquadOnlineHouse2 = 1 << 7,

        /// <summary>
        /// House Balance Member.
        /// </summary>
        HypeSquadOnlineHouse3 = 1 << 8,

        /// <summary>
        /// Early Nitro Supporter.
        /// </summary>
        PremiumEarlySupporter = 1 << 9,

        /// <summary>
        /// User is a team.
        /// </summary>
        TeamPseudoUser = 1 << 10,

        /// <summary>
        /// Bug Hunter Level 2.
        /// </summary>
        BugHunterLevel2 = 1 << 14,

        /// <summary>
        /// Verified Bot.
        /// </summary>
        VerifiedBot = 1 << 16,

        /// <summary>
        /// Early Verified Bot Developer.
        /// </summary>
        VerifiedDeveloper = 1 << 17,

        /// <summary>
        /// Moderator Programs Alumni.
        /// </summary>
        CertifiedModerator = 1 << 18,

        /// <summary>
        /// Bot uses only HTTP interactions and is shown in the online member list.
        /// </summary>
        BotHttpInteractions = 1 << 19,

        /// <summary>
        /// User is an Active Developer.
        /// </summary>
        ActiveDeveloper = 1 << 22
    }

}
