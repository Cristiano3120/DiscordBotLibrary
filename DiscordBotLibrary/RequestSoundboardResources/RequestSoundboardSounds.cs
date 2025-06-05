namespace DiscordBotLibrary.RequestSoundboardResources
{
    internal sealed class RequestSoundboardSounds
    {
        public Dictionary<ulong, TaskCompletionSource<SoundboardSound[]>> TaskCompletionSources { get; init; } = new();

        public ulong[] GuildIds { get; private init; } = [];

        public RequestSoundboardSounds(ulong[] guildIds)
        {
            GuildIds = guildIds;
            foreach (ulong guildId in guildIds)
            {
                TaskCompletionSources.Add(guildId, new TaskCompletionSource<SoundboardSound[]>(TaskCreationOptions.RunContinuationsAsynchronously));
            }
        }
    }
}
