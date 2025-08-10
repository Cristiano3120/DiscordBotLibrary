using System.Collections.Concurrent;
using DiscordBotLibrary.ChannelResources;

namespace DiscordBotLibrary.VoiceChannelHandling
{
    public sealed class VoiceChannelHandler
    {
        private readonly ConcurrentDictionary<ulong, InternVoiceChannelConn> _voiceConnections;
        private readonly ShardHandler _shardHandler;

        internal VoiceChannelHandler(ShardHandler shardHandler)
        {
            _voiceConnections = new ConcurrentDictionary<ulong, InternVoiceChannelConn>();
            _shardHandler = shardHandler;
        }

        private VoiceChannelHandler() { }

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
            };

            Payload<UpdateVoiceState> payload = new(OpCode.VoiceStateUpdate, updateVoiceState);
            await _shardHandler.SendShardSpecificMessageAsync(guildId, payload);
        }

        public async Task MuteAsync(ulong guildId, bool selfMute)
        {
            if (!_voiceConnections.TryGetValue(guildId, out InternVoiceChannelConn? voiceChannelConn))
            {
                return;
            }

            if (voiceChannelConn.SelfMute == selfMute)
            {
                return;
            }

            UpdateVoiceState updateVoiceState = new()
            {
                GuildId = guildId,
                ChannelId = voiceChannelConn.ChannelId,
                SelfDeaf = voiceChannelConn.SelfDeaf,
                SelfMute = selfMute,
            };

            Payload<UpdateVoiceState> payload = new(OpCode.VoiceStateUpdate, updateVoiceState);
            await _shardHandler.SendShardSpecificMessageAsync(guildId, payload);
        }

        public async Task DeafenAsync(ulong guildId, bool selfDeaf)
        {
            if (!_voiceConnections.TryGetValue(guildId, out InternVoiceChannelConn? voiceChannelConn))
            {
                return;
            }

            if (voiceChannelConn.SelfDeaf == selfDeaf)
            {
                return;
            }

            UpdateVoiceState updateVoiceState = new()
            {
                GuildId = guildId,
                ChannelId = voiceChannelConn.ChannelId,
                SelfMute = voiceChannelConn.SelfMute,
                SelfDeaf = selfDeaf,
            };

            Payload<UpdateVoiceState> payload = new(OpCode.VoiceStateUpdate, updateVoiceState);
            await _shardHandler.SendShardSpecificMessageAsync(guildId, payload);
        }

        internal void ReceivedVoiceServerUpdate(VoiceServerUpdate voiceServerUpdate)
        {
            _voiceConnections.TryGetValue(voiceServerUpdate.GuildId, out InternVoiceChannelConn? voiceChannelConn);
            voiceChannelConn?.ReceivedVoiceServerUpdate(voiceServerUpdate);
        }

        internal IReadOnlyDictionary<ulong, VoiceChannelConn> GetVoiceConns()
            => _voiceConnections.ToDictionary(kvp => kvp.Key, kvp => (VoiceChannelConn)kvp.Value);
    }
}
