using System.Net.WebSockets;
using DiscordBotLibrary.Sharding;

namespace DiscordBotLibrary
{
    internal static class HandleDiscordPayload
    {
        internal static async Task<int> HandleDispatch(Shard shard, JToken jToken)
        {
            Event events = jToken.GetEvent();
            await shard.InvokeEvent(events, jToken);

            return jToken.GetSequenceNumber();
        }

        internal static async Task HandleHelloEventAsync(JToken jToken, Shard shard,
            ResumeConnInfos resumeConnInfos, int? lastSequenceNumber)
        {
            int heartbeatInterval = jToken.GetProperty("d").GetProperty("heartbeat_interval").Value<int>();
            _ = shard.SendHeartbeatsAsync(heartbeatInterval);

            if (resumeConnInfos != ResumeConnInfos.EmptyConnInfos)
            {
                DiscordClient.Logger.LogDebug("Resuming connection.");
                var payload = new
                {
                    op = OpCode.Resume,
                    d = new
                    {
                        token = DiscordClient.GetDiscordClient().ClientConfig.Token,
                        session_id = resumeConnInfos.SessionId,
                        seq = lastSequenceNumber
                    }
                };

                await shard.SendPayloadWssAsync(payload);
            }
            else
            {
                await shard.SendIdentifyAsync();
            }
        }

        internal static async Task HandleResumeConnAsync(ClientWebSocket webSocket, ResumeConnInfos resumeConnInfos)
        {
            try
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }

                webSocket = new ClientWebSocket();
                await webSocket.ConnectAsync(resumeConnInfos.ResumeGatewayUri!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                DiscordClient.Logger.LogError(ex);
            }
        }

        internal static async Task HandleSessionInvalidAsync(Shard shard, JToken jToken,
            ResumeConnInfos resumeConnInfos, ClientWebSocket clientWebSocket)
        {
            bool canResume = jToken.GetProperty("d").Value<bool>();

            if (canResume)
            {
                await HandleResumeConnAsync(clientWebSocket, resumeConnInfos);
            }
            else
            {
                await shard.RestartShardAsync();
            }
        }

        internal static void HandleReadyEvent(Shard shard, JToken jToken)
        {
            ShardReadyEventArgs readyEventArgs = jToken.Deserialize<ShardReadyEventArgs>();

            ShardHandler.ShardReady(readyEventArgs);
            shard.ResumeConnInfos = new ResumeConnInfos
            {
                SessionId = readyEventArgs.SessionId,
                ResumeGatewayUri = new Uri(readyEventArgs.ResumeGatewayUri)
            };
        }

        internal static void HandleGuildCreateEvent(JToken jToken)
        {
            DiscordGuild discordGuild = jToken.Deserialize<DiscordGuild>();

            DiscordClient client = DiscordClient.GetDiscordClient();
            client.InternalGuilds.AddOrUpdate(discordGuild.Id, discordGuild, (_, _) => discordGuild);

            client.InvokeOnGuildCreate(discordGuild);
        }

        internal static void HandlePresenceUpdateEvent(JToken jToken)
        {
            Presence presenceUpdate = jToken.Deserialize<Presence>()!;

            DiscordClient client = DiscordClient.GetDiscordClient();
            DiscordGuild guild = client.InternalGuilds[presenceUpdate.GuildId];

            guild.UpdatePresence(presenceUpdate);
            client.InvokeOnPresenceUpdate(presenceUpdate);
        }

        internal static void HandleVoiceServerUpdateEvent(JToken jToken)
        {
            VoiceServerUpdate voiceServerUpdate = jToken.Deserialize<VoiceServerUpdate>();
            DiscordClient.GetDiscordClient().ReceivedVoiceServerUpdate(voiceServerUpdate);
        }

        internal static void HandleGuildMembersChunkEvent(JToken jToken, Shard shard)
        {
            GuildMembersChunk guildMembersChunk = jToken.Deserialize<GuildMembersChunk>();
            shard.GuildMemberRequests.TryGetValue(guildMembersChunk.Nonce, out RequestGuildMembersCache? requestGuildMembersCache);

            if (requestGuildMembersCache is null)
                return;

            requestGuildMembersCache.GuildMembers.AddRange(guildMembersChunk.Members);

            Dictionary<ulong, Presence> presenceMap = guildMembersChunk.Presences?
                .ToDictionary(p => p.User.Id) ?? [];

            foreach (GuildMember member in requestGuildMembersCache.GuildMembers)
            {
                if (member is not null && presenceMap.TryGetValue(member.User?.Id ?? 0, out Presence? presence))
                {
                    member.SetPresence(presence);
                }
            }

            if (guildMembersChunk.ChunkIndex == guildMembersChunk.ChunkCount - 1)
            {
                requestGuildMembersCache.TaskCompletionSource.SetResult(requestGuildMembersCache.GuildMembers);
                shard.GuildMemberRequests.Remove(guildMembersChunk.Nonce);

                if (requestGuildMembersCache.Cache)
                {
                    DiscordClient.GetDiscordClient()
                        .InternalGuilds[guildMembersChunk.GuildId].AddGuildMembers(requestGuildMembersCache.GuildMembers);              
                }
            }
        }

        internal static void HandleSoundboardSoundsEvent(JToken jToken, Shard shard)
        {
            SoundboardSounds soundboardSounds = jToken.Deserialize<SoundboardSounds>();

            shard.SoundboardRequests.TryGetValue(soundboardSounds.GuildId, out TaskCompletionSource<SoundboardSound[]>? tsc);
            if (tsc is null)
                return;

            tsc.SetResult(soundboardSounds.SoundboardSoundsArr);
            shard.SoundboardRequests.Remove(soundboardSounds.GuildId);
        }

        internal static void HandleVoiceStateUpdateEvent(JToken jToken)
        {
            VoiceState voiceState = jToken.Deserialize<VoiceState>();
            
            if (!voiceState.GuildId.HasValue)
                return;

            DiscordClient client = DiscordClient.GetDiscordClient();
            client.GetGuild(voiceState.GuildId.Value)?.UpdateVoiceState(voiceState);
            client.InvokeOnVoiceStateUpdate(voiceState);
        }

        #region HandleChannelEvents

        internal static void HandleChannelCreateEvent(JToken jToken)
        {
            Channel channel = jToken.Deserialize<Channel>();
            DiscordClient client = DiscordClient.GetDiscordClient();

            client.GetGuild(channel.GuildId!.Value)?.AddChannel(channel);
            client.InvokeOnChannelCreated(channel); 
        }

        internal static void HandleChannelDeleteEvent(JToken jToken)
        {
            Channel channel = jToken.Deserialize<Channel>();
            DiscordClient client = DiscordClient.GetDiscordClient();

            client.GetGuild(channel.GuildId!.Value)?.DeleteChannel(channel);
            client.InvokeOnChannelDeleted(channel);
        }

        internal static void HandleChannelUpdateEvent(JToken jToken)
        {
            Channel channel = jToken.Deserialize<Channel>();
            DiscordClient client = DiscordClient.GetDiscordClient();

            client.GetGuild(channel.GuildId!.Value)?.UpdateChannel(channel);
            client.InvokeOnChannelUpdated(channel);
        }

        internal static async Task HandleChannelPinsUpdateEvent(JToken jToken)
        {
            ChannelPinsUpdate channelPinsUpdate = jToken.Deserialize<ChannelPinsUpdate>();
            DiscordClient client = DiscordClient.GetDiscordClient();

            if (channelPinsUpdate.GuildId is ulong guildId)
            {
                ChannelPins channelPins = await client.GetGuild(guildId)!.UpdateChannelPinsAsync(channelPinsUpdate);
                client.InvokeOnChannelPinsUpdate(channelPins);
            }
        }

        #endregion
    }
}
