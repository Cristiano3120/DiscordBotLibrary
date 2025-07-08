namespace DiscordBotLibrary.WssPayloadStructures
{
    internal readonly struct ResumePayload
    {
        public ResumePayload(string token, string sessionId, int? seq)
        {
            Token = token;
            SessionId = sessionId;
            Seq = seq;
        }

        public string Token { get; init; }
        public string SessionId { get; init; }
        public int? Seq { get; init; }
    }
}
