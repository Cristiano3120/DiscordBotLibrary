using System.Collections.Concurrent;

namespace DiscordBotLibrary.VoiceChannelHandling
{
    internal sealed class VoiceChannelHandler
    {
        private readonly ConcurrentDictionary<ulong, InternVoiceChannelConn> _voiceConnections;
        private readonly ShardHandler _shardHandler;

        public VoiceChannelHandler(ShardHandler shardHandler)
        {
            _voiceConnections = new ConcurrentDictionary<ulong, InternVoiceChannelConn>();
            _shardHandler = shardHandler;
        }

        public async Task ConnectToVcAsync(ulong guildId, ulong channelId, bool selfDeaf = false, bool selfMute = false)
        {
            DiscordClient.Logger.Log(LogLevel.Debug, $"Connecting to voice channel {channelId} in guild {guildId} with selfDeaf={selfDeaf} and selfMute={selfMute}");
            UpdateVoiceState updateVoiceState = new()
            {
                GuildId = guildId,
                ChannelId = channelId,
                SelfDeaf = selfDeaf,
                SelfMute = selfMute,
            };

            Payload<UpdateVoiceState> payload = new(OpCode.VoiceStateUpdate, updateVoiceState);
            await _shardHandler.SendShardSpecificMessageAsync(guildId, payload);

            Func<ulong, InternVoiceChannelConn> addFunc
                = (_) => new InternVoiceChannelConn(guildId, channelId, selfDeaf, selfMute);
            Func<ulong, InternVoiceChannelConn, InternVoiceChannelConn> UpdateFunc
                = (_, _) => new InternVoiceChannelConn(guildId, channelId, selfDeaf, selfMute);

            _voiceConnections.AddOrUpdate(guildId, addFunc, UpdateFunc);
        }

        public async Task DisconnectFromVcAsync(ulong guildId)
        {
            if (!_voiceConnections.TryRemove(guildId, out _))
            {
                return;
            }

            UpdateVoiceState updateVoiceState = new()
            {
                GuildId = guildId,
                ChannelId = null,
                SelfDeaf = false,
                SelfMute = false,
            };

            Payload<UpdateVoiceState> payload = new(OpCode.VoiceStateUpdate, updateVoiceState);
            await _shardHandler.SendShardSpecificMessageAsync(guildId, payload);
        }

        public void ReceivedVoiceServerUpdate(VoiceServerUpdate voiceServerUpdate)
        {
            _voiceConnections.TryGetValue(voiceServerUpdate.GuildId, out InternVoiceChannelConn? voiceChannelConn);
            voiceChannelConn?.ReceivedVoiceServerUpdate(voiceServerUpdate);
        }

        public IReadOnlyDictionary<ulong, VoiceChannelConn> GetVoiceConns()
            => _voiceConnections.ToDictionary(kvp => kvp.Key, kvp => (VoiceChannelConn)kvp.Value);
    }
}
