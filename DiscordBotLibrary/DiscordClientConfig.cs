namespace DiscordBotLibrary
{
    public class DiscordClientConfig
    {
        public LogLevel LogLevel { get; init; } = LogLevel.Info;
        public string Token { get; init; } = string.Empty;
        public int Version { get; init; } = 10;
        public Intents Intents { get; init; }

        internal static DiscordClientConfig Default => new()
        {
            Intents = Intents.AllGuildEvents | Intents.AllMessageEvents,
            LogLevel = LogLevel.Info,
            Token = string.Empty,
            Version = 10,
        };
    }
}
