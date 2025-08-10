namespace DiscordBotLibrary.DiscordClientResources
{
    public sealed partial class DiscordClient
    {
        #region Channel Events

        public event Action<DiscordClient, Channel>? OnChannelCreated;
        public event Action<DiscordClient, Channel>? OnChannelDeleted;
        public event Action<DiscordClient, Channel>? OnChannelUpdated;
        public event Action<DiscordClient, ChannelPins>? OnChannelPinsUpdate;

        #endregion

        public event Action<DiscordClient, VoiceState>? OnVoiceStateUpdate;
        public event Action<DiscordClient, DiscordGuild>? OnGuildCreate;
        public event Action<DiscordClient, Presence>? OnPresenceUpdate;

        public event Action<DiscordClient, ReadyEventArgs>? OnReady;
        public event Action<DiscordClient, IReadOnlyDictionary<ulong, DiscordGuild>>? OnGuildsReceived;

        #region InvokeEvents
        internal void InvokeOnGuildCreate(DiscordGuild guild)
            => OnGuildCreate?.Invoke(this, guild);

        internal void InvokeOnPresenceUpdate(Presence presence)
            => OnPresenceUpdate?.Invoke(this, presence);

        internal void InvokeOnVoiceStateUpdate(VoiceState voiceState)
            => OnVoiceStateUpdate?.Invoke(this, voiceState);

        internal void InvokeOnChannelCreated(Channel channel)
            => OnChannelCreated?.Invoke(this, channel);

        internal void InvokeOnChannelDeleted(Channel channel)
            => OnChannelDeleted?.Invoke(this, channel);

        internal void InvokeOnChannelUpdated(Channel channel)
            => OnChannelUpdated?.Invoke(this, channel);

        internal void InvokeOnChannelPinsUpdate(ChannelPins channelPins)
            => OnChannelPinsUpdate?.Invoke(this, channelPins);
        #endregion
    }
}
