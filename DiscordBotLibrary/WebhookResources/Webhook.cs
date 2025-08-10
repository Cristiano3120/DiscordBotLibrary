using System.Runtime.CompilerServices;

namespace DiscordBotLibrary.WebhookResources
{
    public sealed record Webhook
    {
        /// <summary>
        /// the id of the webhook
        /// </summary>
        public ulong Id { get; init; }

        /// <summary>
        /// the type of the webhook
        /// </summary>
        public WebhookType Type { get; init; }

        /// <summary>
        /// the guild id this webhook is for, if any
        /// </summary>
        public ulong? GuildId { get; init; }

        /// <summary>
        /// the channel id this webhook is for, if any
        /// </summary>
        public ulong? ChannelId { get; init; }

        /// <summary>
        /// the user this webhook was created by (not returned when getting a webhook with its token)
        /// </summary>
        public User? User { get; init; }

        /// <summary>
        /// the default name of the webhook
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// the default user avatar hash of the webhook
        /// </summary>
        public string? Avatar { get; init; }

        /// <summary>
        /// the secure token of the webhook (returned for Incoming Webhooks)
        /// </summary>
        public string? Token { get; init; }

        /// <summary>
        /// the bot/OAuth2 application that created this webhook
        /// </summary>
        public ulong? ApplicationId { get; init; }

        /// <summary>
        /// the guild of the channel that this webhook is following (returned for Channel Follower Webhooks)
        /// </summary>
        public Guild? SourceGuild { get; init; }

        /// <summary>
        /// the channel that this webhook is following (returned for Channel Follower Webhooks)
        /// </summary>
        public Channel? SourceChannel { get; init; }

        /// <summary>
        /// the url used for executing the webhook (returned by the webhooks OAuth2 flow)
        /// </summary>
        public string? Url { get; init; }

        private Webhook() { }

        internal static bool CheckWebhookName(string name)
        {
            const byte minLength = 1;
            const byte maxLength = 80;

            if (name.Length is < minLength or > maxLength)
            {
                return false;
            }

            string[] invalidSubstrings = ["clyde", "discord"];
            foreach (string invalidSubstring in invalidSubstrings)
            {
                if (name.Contains(invalidSubstring, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
