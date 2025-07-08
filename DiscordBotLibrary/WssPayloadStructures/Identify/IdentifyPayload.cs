namespace DiscordBotLibrary.WssPayloadStructures.Identify
{
    internal sealed record IdentifyPayload
    {
        public Intents Intents { get; init; }
        public string Token { get; init; } = default!;
        public Properties Properties { get; init; } = default!;
        public int[] Shard { get; init; } = [];
    }
}
