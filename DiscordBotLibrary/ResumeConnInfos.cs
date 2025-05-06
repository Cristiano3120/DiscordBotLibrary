namespace DiscordBotLibrary
{
    /// <summary>
    /// This struct is used to store the information needed to resume a connection.
    /// </summary>
    internal readonly struct ResumeConnInfos
    {
        public string SessionId { get; init; }
        public Uri ResumeGatewayUri { get; init; }
    }
}
