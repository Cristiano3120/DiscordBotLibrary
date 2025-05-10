using System.Net.WebSockets;
using System.Text.Json;
using DiscordBotLibrary.Json;

namespace DiscordBotLibrary
{
    internal static class HandleDiscordPayload
    {
        internal static int HandleDispatch(JsonElement jsonElement, DiscordClient client)
        {
            Events events = jsonElement.GetEvent();
            client.InvokeEvent(events, jsonElement);

            return jsonElement.GetSequenceNumber();
        }

        internal static async Task HandleHelloEvent(JsonElement jsonElement, DiscordClient client, 
            ResumeConnInfos resumeConnInfos, int? lastSequenceNumber)
        {
            client.Logger.LogInfo("Received Hello message.");
            int heartbeatInterval = jsonElement.GetProperty("d").GetProperty("heartbeat_interval").GetInt32();
            _ = client.SendHeartbeatsAsync(heartbeatInterval);

            if (resumeConnInfos != ResumeConnInfos.EmptyConnInfos)
            {
                client.Logger.LogDebug("Resuming connection.");
                var payload = new
                {
                    op = OpCode.Resume,
                    d = new
                    {
                        token = client.ClientConfig.Token,
                        session_id = resumeConnInfos.SessionId,
                        seq = lastSequenceNumber
                    }
                };

                await client.SendPayloadWssAsync(payload);
            }
            else
            {
                await client.SendIdentifyAsync();
            }
        }

        internal static async Task HandleResumeConnAsync(DiscordClient client, ClientWebSocket webSocket, ResumeConnInfos resumeConnInfos)
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
                client.Logger.LogError(ex);
            }
        }
    }
}
