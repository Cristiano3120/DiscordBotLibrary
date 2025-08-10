namespace DiscordBotLibrary.VoiceChannelHandling
{
    internal sealed class InternVoiceChannelConn
    {
        internal ulong GuildId { get; init; }
        internal ulong ChannelId { get; init; }
        internal bool SelfDeaf { get; init; }
        internal bool SelfMute { get; init; }
        internal string Token { get; private set; }
        internal string? Endpoint { get; private set; }


        public InternVoiceChannelConn(ulong guildId, ulong channelId, bool selfDeaf, bool selfMute)
        {
            GuildId = guildId;
            ChannelId = channelId;
            SelfDeaf = selfDeaf;
            SelfMute = selfMute;
            Token = string.Empty;
        }

        public void ReceivedVoiceServerUpdate(VoiceServerUpdate voiceServerUpdate)
        {
            Token = voiceServerUpdate.Token;
            Endpoint = voiceServerUpdate.Endpoint;
        }

        public static explicit operator VoiceChannelConn(InternVoiceChannelConn internVcConn)
           => new(internVcConn);
    }
}
