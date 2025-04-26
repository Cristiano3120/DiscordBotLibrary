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
            Intents = Intents.ALL_GUILD_EVENTS | Intents.ALL_MESSAGE_EVENTS,
            LogLevel = LogLevel.Info,
            Token = string.Empty,
            Version = 10,
        };
    }
}
