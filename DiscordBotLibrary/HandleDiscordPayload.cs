using DiscordBotLibrary.RequestSoundboardResources;
using DiscordBotLibrary.Sharding;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;
using System.Text.Json;

namespace DiscordBotLibrary
{
    internal static class HandleDiscordPayload
    {
        internal static int HandleDispatch(Shard shard, JsonElement jsonElement)
        {
            Event events = jsonElement.GetEvent();
            shard.InvokeEvent(events, jsonElement);

            return jsonElement.GetSequenceNumber();
        }

        internal static async Task HandleHelloEventAsync(JsonElement jsonElement, Shard shard,
            ResumeConnInfos resumeConnInfos, int? lastSequenceNumber)
        {
            int heartbeatInterval = jsonElement.GetProperty("d").GetProperty("heartbeat_interval").GetInt32();
            _ = shard.SendHeartbeatsAsync(heartbeatInterval);

            if (resumeConnInfos != ResumeConnInfos.EmptyConnInfos)
            {
                DiscordClient.Logger.LogDebug("Resuming connection.");
                var payload = new
                {
                    op = OpCode.Resume,
                    d = new
                    {
                        token = DiscordClient.ClientConfig.Token,
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

        internal static async Task HandleSessionInvalidAsync(Shard shard, JsonElement jsonElement,
            ResumeConnInfos resumeConnInfos, ClientWebSocket clientWebSocket)
        {
            bool canResume = jsonElement.GetProperty("d").GetBoolean();

            if (canResume)
            {
                await HandleResumeConnAsync(clientWebSocket, resumeConnInfos);
            }
            else
            {
                await shard.RestartShardAsync();
            }
        }

        internal static void HandleReadyEvent(Shard shard, JsonElement jsonElement)
        {
            ShardReadyEventArgs readyEventArgs = jsonElement.Deserialize<ShardReadyEventArgs>();

            ShardHandler.ShardReady(readyEventArgs);
            shard.ResumeConnInfos = new ResumeConnInfos
            {
                SessionId = readyEventArgs.SessionId,
                ResumeGatewayUri = new Uri(readyEventArgs.ResumeGatewayUri)
            };
        }

        internal static void HandleGuildCreateEvent(JsonElement jsonElement)
        {
            DiscordGuild discordGuild = jsonElement.Deserialize<DiscordGuild>();
            
            DiscordClient client = DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>();
            client.InternalGuilds.AddOrUpdate(discordGuild.Id, discordGuild, (_, _) => discordGuild);

            client.InvokeOnGuildCreate(discordGuild);
        }

        internal static void HandlePresenceUpdateEvent(JsonElement jsonElement)
        {
            Presence presenceUpdate = jsonElement.Deserialize<Presence>()!;

            DiscordClient client = DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>();
            DiscordGuild guild = client.InternalGuilds[presenceUpdate.GuildId];

            guild.UpdatePresence(presenceUpdate);
            client.InvokeOnPresenceUpdate(presenceUpdate);
        }

        internal static void HandleVoiceServerUpdateEvent(JsonElement jsonElement)
        {
            VoiceServerUpdate voiceServerUpdate = jsonElement.Deserialize<VoiceServerUpdate>();
            DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>().ReceivedVoiceServerUpdate(voiceServerUpdate);
        }

        internal static void HandleGuildMembersChunkEvent(JsonElement jsonElement, Shard shard)
        {
            GuildMembersChunk guildMembersChunk = jsonElement.Deserialize<GuildMembersChunk>();
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
                    DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>()
                        .InternalGuilds[guildMembersChunk.GuildId].AddGuildMembers(requestGuildMembersCache.GuildMembers);              
                }
            }
        }

        internal static void HandleSoundboardSoundsEvent(JsonElement jsonElement, Shard shard)
        {
            SoundboardSounds soundboardSounds = jsonElement.Deserialize<SoundboardSounds>();

            shard.SoundboardRequests.TryGetValue(soundboardSounds.GuildId, out TaskCompletionSource<SoundboardSound[]>? tsc);
            if (tsc is null)
                return;

            tsc.SetResult(soundboardSounds.SoundboardSoundsArr);
            shard.SoundboardRequests.Remove(soundboardSounds.GuildId);
        }

        internal static void HandleVoiceStateUpdateEvent(JsonElement jsonElement)
        {
            VoiceState voiceState = jsonElement.Deserialize<VoiceState>();

            if (!voiceState.GuildId.HasValue)
                return;

            DiscordClient client = DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>();
            client.GetGuild(voiceState.GuildId.Value)?.UpdateVoiceState(voiceState);
        }
    }
}
