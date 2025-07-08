using System.Net.WebSockets;

namespace DiscordBotLibrary
{
    internal sealed class HandleDiscordPayload(DiscordClient discordClient, ShardHandler shardHandler)
    {
        private readonly DiscordClient _discordClient = discordClient;
        private readonly ShardHandler _shardHandler = shardHandler;

        internal static async Task<int> HandleDispatch(Shard shard, JToken jToken)
        {
            Event events = jToken.GetEvent();
            await shard.InvokeEvent(events, jToken);
            
            return jToken.GetSequenceNumber();
        }

        internal static async Task HandleHelloEventAsync(HelloEventParams eventParams)
        {
            int heartbeatInterval = eventParams.JToken.GetProperty("d").GetProperty("heartbeat_interval").Value<int>();
            Shard shard = eventParams.Shard;

            _ = shard.SendHeartbeatsAsync(heartbeatInterval);

            ResumeConnInfos resumeConnInfos = eventParams.ResumeConnInfos;
            if (resumeConnInfos != ResumeConnInfos.EmptyConnInfos)
            {
                DiscordClient.Logger.LogDebug("Resuming connection.");

                ResumePayload resumePayload = new(eventParams.Token, resumeConnInfos.SessionId, eventParams.LastSequenceNumber);
                Payload<ResumePayload> payload = new(OpCode.Resume, resumePayload);

                await shard.SendPayloadWssAsync(payload);
            }
            else
            {
                await shard.SendIdentifyAsync(eventParams.ShardCount);
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
            ResumeConnInfos resumeConnInfos, ClientWebSocket _discordClientWebSocket)
        {
            bool canResume = jToken.GetProperty("d").Value<bool>();

            if (canResume)
            {
                await HandleResumeConnAsync(_discordClientWebSocket, resumeConnInfos);
            }
            else
            {
                await shard.RestartShardAsync();
            }
        }

        internal void HandleReadyEvent(Shard shard, JToken jToken)
        {
            ShardReadyEventArgs readyEventArgs = jToken.Deserialize<ShardReadyEventArgs>();

            _shardHandler.ShardReady(readyEventArgs);
            shard.ResumeConnInfos = new ResumeConnInfos
            {
                SessionId = readyEventArgs.SessionId,
                ResumeGatewayUri = new Uri(readyEventArgs.ResumeGatewayUri)
            };
        }

        internal void HandleGuildCreateEvent(JToken jToken)
        {
            DiscordGuild discordGuild = jToken.Deserialize<DiscordGuild>();
            _discordClient.InternalGuilds.AddOrUpdate(discordGuild.Id, discordGuild, (_, _) => discordGuild);
            _discordClient.InvokeOnGuildCreate(discordGuild);
        }

        internal void HandlePresenceUpdateEvent(JToken jToken)
        {
            Presence presenceUpdate = jToken.Deserialize<Presence>()!;
            DiscordGuild guild = _discordClient.InternalGuilds[presenceUpdate.GuildId];

            guild.UpdatePresence(presenceUpdate);
            _discordClient.InvokeOnPresenceUpdate(presenceUpdate);
        }

        internal void HandleVoiceServerUpdateEvent(JToken jToken)
        {
            VoiceServerUpdate voiceServerUpdate = jToken.Deserialize<VoiceServerUpdate>();
            _discordClient.ReceivedVoiceServerUpdate(voiceServerUpdate);
        }

        internal void HandleGuildMembersChunkEvent(JToken jToken, Shard shard)
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
                    _discordClient.InternalGuilds[guildMembersChunk.GuildId]
                        .AddGuildMembers(requestGuildMembersCache.GuildMembers);              
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

        internal void HandleVoiceStateUpdateEvent(JToken jToken)
        {
            VoiceState voiceState = jToken.Deserialize<VoiceState>();
            
            if (!voiceState.GuildId.HasValue)
                return;

            _discordClient.GetGuild(voiceState.GuildId.Value)?.UpdateVoiceState(voiceState);
            _discordClient.InvokeOnVoiceStateUpdate(voiceState);
        }

        #region HandleChannelEvents

        internal void HandleChannelCreateEvent(JToken jToken)
        {
            Channel channel = jToken.Deserialize<Channel>();

            _discordClient.GetGuild(channel.GuildId!.Value)?.AddChannel(channel);
            _discordClient.InvokeOnChannelCreated(channel); 
        }

        internal void HandleChannelDeleteEvent(JToken jToken)
        {
            Channel channel = jToken.Deserialize<Channel>();

            _discordClient.GetGuild(channel.GuildId!.Value)?.DeleteChannel(channel);
            _discordClient.InvokeOnChannelDeleted(channel);
        }

        internal void HandleChannelUpdateEvent(JToken jToken)
        {
            Channel channel = jToken.Deserialize<Channel>();

            _discordClient.GetGuild(channel.GuildId!.Value)?.UpdateChannel(channel);
            _discordClient.InvokeOnChannelUpdated(channel);
        }

        internal async Task HandleChannelPinsUpdateEvent(JToken jToken)
        {
            ChannelPinsUpdate channelPinsUpdate = jToken.Deserialize<ChannelPinsUpdate>();

            if (channelPinsUpdate.GuildId is ulong guildId)
            {
                ChannelPins channelPins = await _discordClient.GetGuild(guildId)!.UpdateChannelPinsAsync(channelPinsUpdate);
                _discordClient.InvokeOnChannelPinsUpdate(channelPins);
            }
        }

        #endregion
    }
}
