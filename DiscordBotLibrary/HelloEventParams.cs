namespace DiscordBotLibrary
{
    internal sealed record HelloEventParams
    {
        public required JToken JToken { get; init; }
        public required Shard Shard { get; init; }
        public required ResumeConnInfos ResumeConnInfos { get; init; }
        public required int? LastSequenceNumber { get; init; }
        public required string Token { get; init; }
        public required int ShardCount { get; init; }
    }
}
