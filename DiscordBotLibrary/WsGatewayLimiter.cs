namespace DiscordBotLibrary
{
    internal static class WsGatewayLimiter
    {
        private static readonly Queue<object> _messageQueue;
        private static byte _sentMessageCount;
    }
}
