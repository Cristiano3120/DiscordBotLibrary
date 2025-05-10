namespace DiscordBotLibrary
{
    /// <summary>
    /// This struct is used to store the information needed to resume a connection.
    /// </summary>
    internal readonly record struct ResumeConnInfos
    {
        public string SessionId { get; init; }
        public Uri? ResumeGatewayUri { get; init; }

        public static ResumeConnInfos EmptyConnInfos => new()
        {
            SessionId = string.Empty,
            ResumeGatewayUri = null
        };
    }
}
