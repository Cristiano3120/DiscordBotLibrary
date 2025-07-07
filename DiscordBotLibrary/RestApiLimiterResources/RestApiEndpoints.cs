namespace DiscordBotLibrary.RestApiLimiterResources
{
    /// <summary>
    /// Contains all the used endpoints
    /// </summary>
    internal static class RestApiEndpoints
    {
        public static string GetChannelEndpoint(ulong channelId, ChannelEndpoint channelEndpoint)
            => channelEndpoint switch
            {
                ChannelEndpoint.Get or ChannelEndpoint.Delete or ChannelEndpoint.Modify => $"channels/{channelId}",
                ChannelEndpoint.Pins => $"channels/{channelId}/pins",
                _ => throw new NotImplementedException("Unsupported ChannelEndpoint")
            };

        public static string GetGuildEndpoint(ulong guildId, ChannelEndpoint channelEndpoint)
            => channelEndpoint switch
            {
                ChannelEndpoint.Delete => $"users/@me/guilds/{guildId}",
                _ => throw new NotImplementedException("Unsupported ChannelEndpoint")
            };
    }
}
