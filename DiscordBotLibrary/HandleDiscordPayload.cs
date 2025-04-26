using System.Text.Json;

namespace DiscordBotLibrary
{
    internal static class HandleDiscordPayload
    {
        internal static int HandleDispatch(JsonElement jsonElement, DiscordClient client)
        {
            Events events = jsonElement.GetEvent();
            client.Logger.LogDebug($"Received Dispatch message: {events}");
            client.InvokeEvent(events, jsonElement);

            return jsonElement.GetSequenceNumber();
        }

        internal static void HandleHelloEvent(JsonElement jsonElement, DiscordClient client)
        {
            client.Logger.LogInfo("Received Hello message.");
            int heartbeatInterval = jsonElement.GetProperty("d").GetProperty("heartbeat_interval").GetInt32();
            _ = client.SendHeartbeatsAsync(heartbeatInterval);
        }
    }
}
