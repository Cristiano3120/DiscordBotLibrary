namespace DiscordBotLibrary.OverwriteResources
{
    public readonly struct Overwrite
    {
        /// <summary>
        /// role or user id
        /// </summary>
        public required ulong Id { get; init; }

        public required OverwriteType Type { get; init; }

        public DiscordPermissions Allow { get; init; }

        public DiscordPermissions Deny { get; init; }
    }
}
