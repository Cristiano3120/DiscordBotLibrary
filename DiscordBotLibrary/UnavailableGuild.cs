namespace DiscordBotLibrary
{
    internal readonly struct UnavailableGuild
    {
        public string Id { get; init; }
        public bool Unavailable { get; init; }
    }
}