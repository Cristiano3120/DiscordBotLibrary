namespace DiscordBotLibrary.ChannelResources.StartThreadParamsResources
{
    public sealed record StartThreadParams : StartThreadFromMessageParams
    {
        /// <summary>
        /// Can only be <see cref="ChannelType.PublicThread"/>
        /// , <see cref="ChannelType.PrivateThread"/> or <see cref="ChannelType.AnnouncementThread"/>"/>
        /// 
        /// <para>
        /// Currently (API v.10) Type always defaults to <see cref="ChannelType.PrivateThread"/>
        /// </para>
        /// </summary>
        public ChannelType Type { get; init; }

        /// <summary>
        /// Whether non-moderators can add other non-moderators to a thread; only available when creating a private thread
        /// </summary>
        public bool? Invitable { get; init; }
    }
}
