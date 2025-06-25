namespace DiscordBotLibrary.Sharding.Identify
{
    internal sealed record IdentifyData
    {
        public Intents Intents { get; init; }
        public string Token { get; init; } = default!;
        public Properties Properties { get; init; } = default!;
        public int[] Shard { get; init; } = [];
    }
}
