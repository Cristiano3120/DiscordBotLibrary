namespace DiscordBotLibrary.ExternalExtraClasses
{
    public sealed record ChannelPins
    {
        public Channel Channel { get; init; } = default!;
        public Message[] PinnedMessages { get; init; } = default!;
    }
}
