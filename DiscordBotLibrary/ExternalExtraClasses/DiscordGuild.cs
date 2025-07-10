using System.Diagnostics;
using System.Runtime.Serialization;
using DiscordBotLibrary.ChannelResources.ChannelEnums;

namespace DiscordBotLibrary.ExternalExtraClasses
{
    /// <summary>
    /// Represents a Discord guild.
    /// If <c>Unavailable</c> is <c>true</c> most of the properties will be <c>null</c>.
    /// </summary>
    public sealed class DiscordGuild : Guild
    {
        #region Constructors
        internal DiscordGuild(UnavailableGuild unavailableGuild)
        {
            Id = unavailableGuild.Id;
            Unavailable = unavailableGuild.Unavailable;
        }

        private DiscordGuild() { }

        #endregion

        #region GuildCreateExtraProperties

        /// <summary>
        /// Gets the date and time when this guild was joined.
        /// </summary>
        public DateTime JoinedAt { get; init; }

        /// <summary>
        /// Gets a value indicating whether this is considered a large guild.
        /// </summary>
        public bool Large { get; init; }

        /// <summary>
        /// Gets a value indicating whether this guild is unavailable due to an outage.
        /// </summary>
        public bool? Unavailable { get; init; }

        /// <summary>
        /// Gets the total number of members in this guild.
        /// </summary>
        public int MemberCount { get; init; }

        /// <summary>
        /// Gets the voice states of members currently in voice channels.
        /// These states do not include the guild_id key.
        /// </summary>
        [JsonProperty]
        internal VoiceState[]? VoiceStates { get; set; }

        /// <summary>
        /// Gets the users currently in the guild.
        /// </summary>
        public List<GuildMember> Members { get; init; } = [];

        /// <summary>
        /// Gets the channels in the guild.
        /// </summary>
        [JsonProperty]
        internal List<Channel> Channels { get; set; } = [];

        /// <summary>
        /// Gets all active threads in the guild that the current user has permission to view.
        /// </summary>
        public Channel[] Threads { get; init; } = [];

        /// <summary>
        /// Gets the presence updates of the members in the guild.
        /// Will only include non-offline members if the size is greater than the large threshold.
        /// </summary>
        public List<Presence> Presences { get; init; } = [];

        /// <summary>
        /// Gets the stage instances in the guild.
        /// </summary>
        public StageInstance[] StageInstances { get; init; } = [];

        /// <summary>
        /// Gets the scheduled events in the guild.
        /// </summary>
        public GuildScheduledEvent[] GuildScheduledEvents { get; init; } = [];

        /// <summary>
        /// Gets the soundboard sounds in the guild.
        /// </summary>
        public SoundboardSound[] SoundboardSounds { get; internal set; } = [];

        #endregion

        #region Channels

        [JsonIgnore]
        /// <summary>
        /// A text channel within a server.
        /// TYPE: <see cref="ChannelType.GuildText"/>
        /// </summary>
        public Channel[] TextChannels => [.. Channels.Where(x => x.Type == ChannelType.Text)];

        [JsonIgnore]
        /// <summary>
        /// A voice channel within a server.
        /// TYPE: <see cref="ChannelType.GuildVoice"/>
        /// </summary>
        public Channel[] VoiceChannels => [.. Channels.Where(x => x.Type == ChannelType.Voice)];

        [JsonIgnore]
        /// <summary>
        /// An organizational category that contains up to 50 channels.
        /// TYPE: <see cref="ChannelType.GuildCategory"/>
        /// </summary>
        public Channel[] Categories => [.. Channels.Where(x => x.Type == ChannelType.Category)];

        [JsonIgnore]
        /// <summary>
        /// A channel that users can follow and crosspost into their own server.
        /// TYPE: <see cref="ChannelType.GuildAnnouncement"/>
        /// </summary>
        public Channel[] AnnouncementChannels => [.. Channels.Where(x => x.Type == ChannelType.Announcement)];

        [JsonIgnore]
        /// <summary>
        /// A temporary sub-channel within a GuildAnnouncement channel.
        /// TYPE: <see cref="ChannelType.AnnouncementThread"/>
        /// </summary>
        public Channel[] AnnouncementThreads => [.. Channels.Where(x => x.Type == ChannelType.AnnouncementThread)];

        [JsonIgnore]
        /// <summary>
        /// A temporary sub-channel within a GuildText or GuildForum channel.
        /// TYPE: <see cref="ChannelType.PublicThread"/>
        /// </summary>
        public Channel[] PublicThreads => [.. Channels.Where(x => x.Type == ChannelType.PublicThread)];

        [JsonIgnore]
        /// <summary>
        /// A temporary sub-channel within a GuildText channel with limited access.
        /// TYPE: <see cref="ChannelType.PrivateThread"/>
        /// </summary>
        public Channel[] PrivateThreads => [.. Channels.Where(x => x.Type == ChannelType.PrivateThread)];

        [JsonIgnore]
        /// <summary>
        /// A voice channel for hosting events with an audience.
        /// TYPE: <see cref="ChannelType.GuildStageVoice"/>
        /// </summary>
        public Channel[] StageChannels => [.. Channels.Where(x => x.Type == ChannelType.StageVoice)];

        [JsonIgnore]
        /// <summary>
        /// The channel in a hub containing the listed servers.
        /// TYPE: <see cref="ChannelType.GuildDirectory"/>
        /// </summary>
        public Channel[] DirectoryChannels => [.. Channels.Where(x => x.Type == ChannelType.Directory)];

        [JsonIgnore]
        /// <summary>
        /// A channel that can only contain threads.
        /// TYPE: <see cref="ChannelType.GuildForum"/>
        /// </summary>
        public Channel[] ForumChannels => [.. Channels.Where(x => x.Type == ChannelType.Forum)];

        [JsonIgnore]
        /// <summary>
        /// A channel that can only contain threads, similar to GuildForum.
        /// TYPE: <see cref="ChannelType.GuildMedia"/>
        /// </summary>
        public Channel[] MediaChannels => [.. Channels.Where(x => x.Type == ChannelType.Media)];
        #endregion

        #region Events

        public event Action<DiscordGuild, VoiceState>? OnVoiceStateUpdated;
        public event Action<DiscordGuild, Presence>? OnPresenceUpdated;

        #region ChannelEvents
        public event Action<DiscordGuild, ChannelPins>? OnChannelPinsUpdated;

        public event Action<DiscordGuild, Channel>? OnChannelCreated;
        public event Action<DiscordGuild, Channel>? OnChannelUpdated;
        public event Action<DiscordGuild, Channel>? OnChannelDeleted;
        #endregion

        #endregion

        #region OnDeserialized

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            SortVoiceStatesAccordingToChannel();
            SetChannelsGuildId();
        }

        private void SortVoiceStatesAccordingToChannel()
        {
            if (VoiceStates is null || VoiceStates.Length == 0)
                return;
            
            Dictionary<ulong, Channel> channels = Channels.ToDictionary(c => c.Id, c => c);
            foreach (VoiceState voiceState in VoiceStates)
            {
                Channel channel = channels[voiceState.ChannelId!.Value];
                channel.VoiceStates ??= [];
                channel.VoiceStates.Add(voiceState);
            }

            VoiceStates = null;
        }

        private void SetChannelsGuildId()
        {
            foreach (Channel channel in Channels)
            {
                channel.GuildId = Id;
            }
        }

        #endregion

        #region GetMethods

        public async Task<SoundboardSound[]> GetSoundboardSoundsAsync()
        {
            if (SoundboardSounds is null || SoundboardSounds.Length > 0)
            {
                SoundboardSounds = await DiscordClient.GetDiscordClient().GetSoundboardSoundsAsync(Id);
            }

            return SoundboardSounds;
        }

        public GuildMember? GetMember(ulong userId)
            => Members.FirstOrDefault(x => x.User?.Id == userId);

        /// <summary>
        /// Could return null even tho the channel exists in <c>rare</c> cases if the channel isnt cached.
        /// I recommend that you call this method <c>first</c> and only after that call the async one if needed
        /// </summary>
        public Channel? GetChannel(ulong channelId)
            => Channels.FirstOrDefault(x => x.Id == channelId);

        /// <summary>
        /// Could return null even tho the channel exists in <c>rare</c> cases if the channel isnt cached.
        /// I recommend that you call this method <c>first</c> and only after that call the async one if needed
        /// </summary>
        public Channel? GetChannel(Func<Channel, bool> predicate)
            => Channels.FirstOrDefault(predicate);

        /// <summary>
        /// Will make an <c>api request</c> if the channel is not found in the cache
        /// </summary>
        public async Task<Channel?> GetChannelAsync(ulong channelId)
        {
            Channel? channel = Channels.FirstOrDefault(x => x.Id == 0);
            if (channel is null)
            {
                string endpoint = RestApiEndpoints.GetChannelEndpoint(channelId, ChannelEndpoint.Get);
                channel = await DiscordClient.GetDiscordClient().RestApiLimiter.GetAsync<Channel>(endpoint, CallerInfos.Create());
            }

            return channel;
        }

        #endregion

        #region VoiceChannelHandling

        /// <summary>
        /// Connects the bot to a voice channel in a specific guild.
        /// <c>True if succesful</c>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> JoinVoiceChannelAsync(ulong channelId, bool selfDeaf = false, bool selfMute = false)
        {
            Channel? channel = GetChannel(channelId);
            if (channel is null || channel.Type is not ChannelType.Voice and not ChannelType.StageVoice || channel.GuildId != channel.Id)
            {
                DiscordClient.Logger.LogError($"The channel either doesnt exist in this guild or is not a voice/stage channel"
                    , CallerInfos.Create());
                return false;
            }

            await DiscordClient.GetDiscordClient()
                .JoinVoiceChannelAsync(Id, channelId, selfDeaf, selfMute);

            return true;
        }

        /// <summary>
        /// Connects the bot to a voice channel in a specific guild.
        /// <c>True if succesful</c>
        /// </summary>
        /// <returns></returns>
        public async Task<bool> JoinVoiceChannelAsync(Channel channel, bool selfDeaf = false, bool selfMute = false)
        {
            if (channel.Type is not ChannelType.Voice and not ChannelType.StageVoice || channel.GuildId != channel.Id)
            {
                DiscordClient.Logger.LogError($"The channel either doesnt exist in this guild or is not a voice/stage channel"
                    , CallerInfos.Create());
                return false;
            }

            await DiscordClient.GetDiscordClient()
                .JoinVoiceChannelAsync(Id, channel.Id, selfDeaf, selfMute);

            return true;
        }

        /// <summary>
        /// Disconnects the bot from the voice channel in a specific guild.
        /// </summary>
        /// <returns></returns>
        public async Task LeaveVoiceChannelAsync()
           => await DiscordClient.GetDiscordClient().LeaveVoiceChannelAsync(Id);

        /// <summary>
        /// Only works on channels that a part of a guild.
        /// </summary>
        /// <returns></returns>
        public Channel? GetChannelThatUserIsIn(ulong userId)
        {
            foreach (Channel channel in VoiceChannels)
            {
                if (channel.VoiceStates is null)
                    continue;

                foreach (VoiceState voiceState in channel.VoiceStates)
                {
                    if (voiceState.UserId == userId)
                        return channel;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the voice state of the specified user in this guild.
        /// </summary>
        public VoiceState? GetVoiceState(ulong userId)
        {
            foreach (Channel channel in VoiceChannels)
            {
                if (channel.VoiceStates is null)
                    continue;

                VoiceState? voiceState = channel.VoiceStates.FirstOrDefault(x => x.UserId == userId);
                if (voiceState is not null)
                    return voiceState;
            }

            return null;
        }

        #endregion

        #region Update Methods(Internal)

        internal void AddGuildMembers(List<GuildMember> guildMembers)
        {
            Dictionary<ulong, int> idToIndex = new(Members.Count);
            for (int i = 0; i < Members.Count; i++)
            {
                if (Members[i].User is null)
                    continue;

                idToIndex[Members[i].User!.Id] = i;
            }

            foreach (GuildMember member in guildMembers)
            {
                if (member.User is null)
                    continue;

                if (idToIndex.TryGetValue(member.User.Id, out int index))
                {
                    Members[index] = member;
                }
                else
                {
                    idToIndex[member.User.Id] = Members.Count;
                    Members.Add(member);
                }
            }
        }

        internal void UpdatePresence(Presence presenceUpdate)
        {
            OnPresenceUpdated?.Invoke(this, presenceUpdate);

            for (int i = 0; i < Presences.Count; i++)
            {
                if (Presences[i].User.Id == presenceUpdate.User.Id)
                {
                    if (presenceUpdate.Status == PresenceStatus.Offline)
                    {
                        Presences.RemoveAt(i);
                    }
                    else
                    {
                        Presences[i] = presenceUpdate;
                    }

                    return;
                }
            }

            if (presenceUpdate.Status != PresenceStatus.Offline)
            {
                Presences.Add(presenceUpdate);
            }
        }

        internal void UpdateVoiceState(VoiceState voiceState)
        {
            OnVoiceStateUpdated?.Invoke(this, voiceState);

            if (voiceState.ChannelId is null)
            {
                Channel? leftChannel = Channels.FirstOrDefault(x => x.VoiceStates?.FirstOrDefault(x => x.UserId == voiceState.UserId) is not null);
                leftChannel?.VoiceStates?.RemoveAll(x => x.UserId == voiceState.UserId);

                return;
            }

            foreach (Channel vc in VoiceChannels)
            {
                Dictionary<ulong, VoiceState> voiceStates = vc.VoiceStates?.ToDictionary(x => x.UserId) ?? [];
                VoiceState? oldVoiceState = voiceStates.GetValueOrDefault(voiceState.UserId);
                if (oldVoiceState is not null)
                {
                    vc.VoiceStates?.Remove(oldVoiceState);
                    break;
                }
            }

            Channel? channel = GetChannel(voiceState.ChannelId.Value);
            channel?.VoiceStates ??= [];
            channel?.VoiceStates?.Add(voiceState);
        }

        #endregion

        #region ChannelEventsHandling

        internal void AddChannel(Channel channel)
        {
            DiscordClient.Logger.Log(LogLevel.Info, $"Adding channel: {channel.Name} to guild: {Name}");
            OnChannelCreated?.Invoke(this, channel);
            Channels.Add(channel);
        }

        internal void DeleteChannel(Channel channel)
        {
            DiscordClient.Logger.Log(LogLevel.Info, $"Deleting channel: {channel.Name} from guild: {Name}");
            OnChannelDeleted?.Invoke(this, channel);
            Channels.RemoveAll(x => x.Id == channel.Id);
        }

        internal void UpdateChannel(Channel channel)
        {
            DiscordClient.Logger.Log(LogLevel.Info, $"Updating channel: {channel.Name} from guild: {Name}");
            OnChannelUpdated?.Invoke(this, channel);

            int index = Channels.FindIndex(x => x.Id == channel.Id);
            Channels[index] = channel;
        }

        internal async Task<ChannelPins> UpdateChannelPinsAsync(ChannelPinsUpdate channelPinsUpdate)
        {
            Channel channel = GetChannel(channelPinsUpdate.ChannelId)!;
            channel.LastPinTimestamp = channelPinsUpdate.LastPinTimestamp;

            Message[] pinnedMessages = await channel.GetPinnedMessagesAsync() ?? [];
            ChannelPins channelPins = new()
            {
                Channel = channel,
                PinnedMessages = pinnedMessages
            };

            OnChannelPinsUpdated?.Invoke(this, channelPins);
            return channelPins;
        }

        /// <summary>
        /// If this method returns null the param is invalid
        /// </summary>
        internal async Task<Message[]?> GetPinnedMessagesAsync(ulong channelId)
        {
            Channel? channel = GetChannel(channelId);
            return channel is not null
                ? await channel.GetPinnedMessagesAsync()
                : null;
        }

        #endregion   
    }
}
