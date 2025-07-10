namespace DiscordBotLibrary
{
    public sealed class DiscordClientConfig
    {
        public string Token { get; init; } = string.Empty;
        public int Version { get; init; } = 10;
        public Intents Intents { get; init; }

        internal static DiscordClientConfig Default => new()
        {
            Intents = Intents.AllGuildEvents | Intents.AllMessageEvents,
            Token = string.Empty,
            Version = 10,
        };
    }
}
