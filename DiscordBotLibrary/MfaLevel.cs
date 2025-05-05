namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the level of multi-factor authentication (MFA) required for a user.
    /// </summary>
    public enum MfaLevel : byte
    {
        None = 0,
        Elevated = 1
    }
}
