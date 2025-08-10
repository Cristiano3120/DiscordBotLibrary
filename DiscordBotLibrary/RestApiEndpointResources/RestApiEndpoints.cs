using DiscordBotLibrary.MessageResources;
using DiscordBotLibrary.RestApiEndpointResources.EndpointEnums;

namespace DiscordBotLibrary.RestApiEndpointResources
{
    /// <summary>
    /// Contains all the used endpoints
    /// </summary>
    internal static class RestApiEndpoints
    {

        #region Guild Endpoints
        public static string GetGuildEndpoint(ulong guildId, GuildSubresource guildSubresource)
        {
            if (guildSubresource == GuildSubresource.None)
            {
                return $"guilds/{guildId}";
            }

            string baseUrl = $"guilds/{guildId}/{guildSubresource.ToString().ToLower()}";
            return baseUrl.Replace('_', '-');
        }

        public static string GetGuildEndpoint(
            ulong guildId,
            GuildSubresource guildSubresource,
            string dynamicPart)
        {
            string baseUrl = GetGuildEndpoint(guildId, guildSubresource);
            return $"{baseUrl}/{dynamicPart}";
        }

        public static string GetGuildEndpoint(
            ulong guildId,
            GuildSubresource guildSubresource,
            string? dynamicPart = null,
            Dictionary<string, string>? queryParams = null)
        {
            string baseUrl = GetGuildEndpoint(guildId, guildSubresource);

            if (!string.IsNullOrWhiteSpace(dynamicPart))
            {
                baseUrl += $"/{Uri.EscapeDataString(dynamicPart)}";
            }

            return AddQueryParams(baseUrl, queryParams);
        }

        #endregion

        #region Channel Endpoints

        public static string GetChannelEndpoint(ulong channelId, ChannelSubresource channelSubresource)
        {
            if (channelSubresource == ChannelSubresource.None)
            {
                return $"channels/{channelId}";
            }

            string baseUrl = $"channels/{channelId}/{channelSubresource.ToString().ToLower()}";
            return baseUrl.Replace('_', '-');
        }

        public static string GetChannelEndpoint(
            ulong channelId,
            ChannelSubresource channelSubresource,
            string dynamicPart)
        {
            string baseUrl = GetChannelEndpoint(channelId, channelSubresource);
            return $"{baseUrl}/{dynamicPart}";
        }

        public static string GetChannelEndpoint(
            ulong channelId,
            ChannelSubresource channelSubresource,
            string? dynamicPart = null,
            Dictionary<string, string>? queryParams = null)
        {
            string baseUrl = GetChannelEndpoint(channelId, channelSubresource);

            if (!string.IsNullOrWhiteSpace(dynamicPart))
            {
                baseUrl += $"/{Uri.EscapeDataString(dynamicPart)}";
            }

            return AddQueryParams(baseUrl, queryParams);
        }

        #endregion

        private static string AddQueryParams(string baseUrl, Dictionary<string, string>? queryParams)
        {
            if (queryParams is { Count: > 0 })
            {
                IEnumerable<string> encoded = queryParams
                    .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");

                baseUrl += "?" + string.Join("&", encoded);
            }

            return baseUrl;
        }

        public static string GetInviteEndpoint(string inviteCode, HttpRequestType requestType)
            => requestType switch
            {
                HttpRequestType.Get or HttpRequestType.Delete => $"invites/{inviteCode}",
                _ => throw new NotImplementedException("Unsupported InviteEndpoint")
            };

        public static string GetWebhookEndpoint(ulong webhookId, HttpRequestType requestType, string? token = null)
            => requestType switch
            {
                HttpRequestType.Delete when token is not null => $"webhooks/{webhookId}/{token}",
                HttpRequestType.Delete => $"webhooks/{webhookId}",
                _ => throw new NotImplementedException("Unsupported WebhookEndpoint")
            };
    }
}
