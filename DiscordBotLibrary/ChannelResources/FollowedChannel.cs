namespace DiscordBotLibrary.ChannelResources
{
    /// <summary> A channel of type <see cref="ChannelType.Announcement"/> that the bot follows</summary>
    public readonly struct FollowedChannel
    {
        public ulong ChannelId { get; init; }

        /// <summary> Created target webhook id </summary>
        public ulong WebhookId { get; init; }
    }
}
