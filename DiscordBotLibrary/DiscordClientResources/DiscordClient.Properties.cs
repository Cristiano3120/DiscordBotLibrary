namespace DiscordBotLibrary.DiscordClientResources
{
    public sealed partial class DiscordClient
    {
        public User? CurrentUser { get; internal set; }
        public Application? Application { get; internal set; }
        public IReadOnlyDictionary<ulong, DiscordGuild> Guilds => InternalGuilds;
        public IReadOnlyDictionary<ulong, VoiceChannelConn> VoiceConnections
            => VoiceChannelHandler.GetVoiceConns();
    }
}
