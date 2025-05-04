namespace DiscordBotLibrary
{
    public record Emoji
    {
        public string? Id { get; init; }
        public string? Name { get; init; }
        public string[]? Roles { get; init; }
        public User? User { get; init; }
        public bool? RequireColons { get; init; }
        public bool? Managed { get; init; }
        public bool? Animated { get; init; }
        public bool? Available { get; init; }
    }
}
