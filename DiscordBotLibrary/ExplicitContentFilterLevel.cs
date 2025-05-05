namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the explicit content filter level for a Discord server.
    /// </summary>
    public enum ExplicitContentFilterLevel : byte
    {
        Disabled = 0,
        MembersWithoutRoles = 1,
        AllMembers = 2
    }
}
