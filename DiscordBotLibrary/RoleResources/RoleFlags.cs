namespace DiscordBotLibrary.RoleResources
{
    public enum RoleFlags : byte
    {
        /// <summary>
        /// No special flags are set for this role.
        /// </summary>
        None = 0,

        /// <summary>
        /// Role can be selected by members in an onboarding prompt
        /// </summary>
        InPrompt = 1 << 0,
    }
}
