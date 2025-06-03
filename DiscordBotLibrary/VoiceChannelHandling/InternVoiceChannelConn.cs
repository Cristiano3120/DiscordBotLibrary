namespace DiscordBotLibrary.VoiceChannelHandling
{
    internal sealed class InternVoiceChannelConn
    {
        private readonly ulong _guildId;
        private readonly ulong _channelId;
        private readonly bool _selfDeaf;
        private readonly bool _selfMute;
        private string _token;
        private string? _endpoint;


        public InternVoiceChannelConn(ulong guildId, ulong channelId, bool selfDeaf, bool selfMute)
        {
            _guildId = guildId;
            _channelId = channelId;
            _selfDeaf = selfDeaf;
            _selfMute = selfMute;
            _token = string.Empty;
        }

        public void ReceivedVoiceServerUpdate(VoiceServerUpdate voiceServerUpdate)
        {
            _token = voiceServerUpdate.Token;
            _endpoint = voiceServerUpdate.Endpoint;
        }

        public static explicit operator VoiceChannelConn(InternVoiceChannelConn internVcConn)
           => new(internVcConn);
    }
}
