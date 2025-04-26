namespace DiscordBotLibrary
{
    internal readonly struct ResumeConnInfos
    {
        public string SessionId { get; init; }
        public Uri ResumeGatewayUri { get; init; }
    }
}
