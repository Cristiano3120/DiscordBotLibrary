namespace DiscordBotLibrary.OverwriteResources
{
    public readonly struct Overwrite
    {
        public ulong Id { get; init; }

        public OverwriteType Type { get; init; }

        public DiscordPermissions Allow { get; init; }

        public DiscordPermissions Deny { get; init; }
    }
}
