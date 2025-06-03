using System.Collections.Concurrent;
using DiscordBotLibrary.Sharding;

namespace DiscordBotLibrary.VoiceChannelHandling
{
    internal class VoiceChannelHandler
    {
        internal ConcurrentDictionary<ulong, InternVoiceChannelConn> VoiceConnections { get; private set; } = new();

        public async Task ConnectToVcAsync(ulong guildId, ulong channelId, bool selfDeaf = false, bool selfMute = false)
        {
            DiscordClient.Logger.LogDebug($"Connecting to voice channel {channelId} in guild {guildId} with selfDeaf={selfDeaf} and selfMute={selfMute}");
            UpdateVoiceState updateVoiceState = new()
            {
                GuildId = guildId,
                ChannelId = channelId,
                SelfDeaf = selfDeaf,
                SelfMute = selfMute,
            };

            var payload = new
            {
                op = OpCode.VoiceStateUpdate,
                d = updateVoiceState,
            };

            await ShardHandler.SendShardSpecificMessageAsync(guildId, payload);

            Func<ulong, InternVoiceChannelConn> addFunc
                = (_) => new InternVoiceChannelConn(guildId, channelId, selfDeaf, selfMute);
            Func<ulong, InternVoiceChannelConn, InternVoiceChannelConn> UpdateFunc
                = (_, _) => new InternVoiceChannelConn(guildId, channelId, selfDeaf, selfMute);

            VoiceConnections.AddOrUpdate(guildId, addFunc, UpdateFunc);
        }

        public async Task DisconnectFromVcAsync(ulong guildId)
        {
            UpdateVoiceState updateVoiceState = new()
            {
                GuildId = guildId,
                ChannelId = null,
                SelfDeaf = false,
                SelfMute = false,
            };

            var payload = new
            {
                op = OpCode.VoiceStateUpdate,
                d = updateVoiceState,
            };

            await ShardHandler.SendShardSpecificMessageAsync(guildId, payload);
            VoiceConnections.TryRemove(guildId, out _);
        }

        public void ReceivedVoiceServerUpdate(VoiceServerUpdate voiceServerUpdate)
        {
            VoiceConnections.TryGetValue(voiceServerUpdate.GuildId, out InternVoiceChannelConn? voiceChannelConn);
            voiceChannelConn?.ReceivedVoiceServerUpdate(voiceServerUpdate);
        }
    }
}
