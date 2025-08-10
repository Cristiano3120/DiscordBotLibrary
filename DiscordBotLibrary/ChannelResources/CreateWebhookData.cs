namespace DiscordBotLibrary.ChannelResources
{
    internal sealed record CreateWebhookData
    {
        public required string Name { get; init; }
        public string? ImageBase64 { get; init; }
    }
}
