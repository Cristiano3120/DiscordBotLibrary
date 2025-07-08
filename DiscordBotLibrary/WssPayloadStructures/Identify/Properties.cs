namespace DiscordBotLibrary.WssPayloadStructures.Identify
{
    internal readonly struct Properties
    {
        public string Os { get; init; }
        public string Browser { get; init; }
        public string Device { get; init; }
    }
}
