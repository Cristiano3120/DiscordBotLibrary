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
            Events events = jsonElement.GetEvent();
            shard.InvokeEvent(events, jsonElement);

            return jsonElement.GetSequenceNumber();
        }

        internal static async Task HandleHelloEvent(JsonElement jsonElement, Shard shard,
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
            DiscordClient client = DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>();
            ReadyEventArgs readyEventArgs = jsonElement.GetProperty("d")
                .Deserialize<ReadyEventArgs>(DiscordClient.JsonSerializerOptions)!;

            client.InvokeOnReady(readyEventArgs);

            shard.ResumeConnInfos = new ResumeConnInfos
            {
                SessionId = readyEventArgs.SessionId,
                ResumeGatewayUri = new Uri(readyEventArgs.ResumeGatewayUri)
            };
        }

        internal static void HandleGuildCreateEvent(JsonElement jsonElement)
        {
            JsonElement data = jsonElement.GetProperty("d");
            IGuildCreateEventArgs guildCreateEventArgs = data.GetProperty("unavailable").GetBoolean()
                ? data.Deserialize<UnavailableGuildCreateEventArgs>(DiscordClient.JsonSerializerOptions)!
                : data.Deserialize<GuildCreateEventArgs>(DiscordClient.JsonSerializerOptions)!;

            GuildCreateEventArgs? guildCreate = guildCreateEventArgs.TryGetAvailableGuild();
            DiscordGuild discordGuild = guildCreate is null
                ? new DiscordGuild(guildCreateEventArgs.TryGetUnavailableGuild()!)
                : new DiscordGuild(guildCreate);

            DiscordClient client = DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>();
            client.InternalGuilds.AddOrUpdate(discordGuild.Id, discordGuild, (_, _) => discordGuild);
            client.InvokeOnGuildCreate(discordGuild);
        }

        internal static void HandlePresenceUpdateEvent(JsonElement jsonElement)
        {
            PresenceUpdate presenceUpdate = jsonElement.GetProperty("d")
                .Deserialize<PresenceUpdate>(DiscordClient.JsonSerializerOptions)!;

            DiscordClient client = DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>();
            DiscordGuild guild = client.InternalGuilds[presenceUpdate.GuildId];

            guild.UpdateUser(presenceUpdate);
            client.InvokeOnPresenceUpdate(presenceUpdate);
        }
    }
}
