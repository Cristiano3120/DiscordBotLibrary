using System.Runtime.Serialization;

namespace DiscordBotLibrary.TeamResources
{
    /// <summary>
    /// Represents the role of a team member in a Discord team.
    /// </summary>
    public enum TeamMemberRole : byte
    {
        /// <summary>
        /// Owners are the most permissible role, and can take destructive, irreversible actions like deleting team-owned apps or the team itself. Teams are limited to 1 owner.
        /// </summary>
        [JsonStringEnumMemberName("owner")]
        Owner,

        /// <summary>
        /// Admins have similar access as owners, except they cannot take destructive actions on the team or team-owned apps.
        /// </summary>
        [JsonStringEnumMemberName("admin")]
        Admin,

        /// <summary>
        /// Developers can access information about team-owned apps, like the client secret or public key. They can also take limited actions on team-owned apps, like configuring interaction endpoints or resetting the bot token.
        /// Members with the Developer role cannot manage the team or its members, or take destructive actions on team-owned apps.
        /// </summary>
        [JsonStringEnumMemberName("developer")]
        Developer,

        /// <summary>
        /// Read-only members can access information about a team and any team-owned apps. Some examples include getting the IDs of applications and exporting payout records.
        /// Members can also invite bots associated with team-owned apps that are marked private.
        /// </summary>
        [JsonStringEnumMemberName("read_only")]
        ReadOnly
    }
}