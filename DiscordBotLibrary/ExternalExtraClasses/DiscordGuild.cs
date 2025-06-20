using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBotLibrary.ExternalExtraClasses
{
    /// <summary>
    /// Represents a Discord guild.
    /// If <c>Unavailable</c> is <c>true</c> most of the properties will be <c>null</c>.
    /// </summary>
    public sealed class DiscordGuild : Guild
    {
        #region GuildCreateExtraProperties

        /// <summary>
        /// Gets the date and time when this guild was joined.
        /// </summary>
        [JsonPropertyName("joined_at")]
        public DateTime JoinedAt { get; init; }

        /// <summary>
        /// Gets a value indicating whether this is considered a large guild.
        /// </summary>
        [JsonPropertyName("large")]
        public bool Large { get; init; }

        /// <summary>
        /// Gets a value indicating whether this guild is unavailable due to an outage.
        /// </summary>
        [JsonPropertyName("unavailable")]
        public bool? Unavailable { get; init; }

        /// <summary>
        /// Gets the total number of members in this guild.
        /// </summary>
        [JsonPropertyName("member_count")]
        public int MemberCount { get; init; }

        /// <summary>
        /// Gets the voice states of members currently in voice channels.
        /// These states do not include the guild_id key.
        /// </summary>
        [JsonPropertyName("voice_states")]
        [JsonInclude]
        internal VoiceState[]? VoiceStates { get; set; }

        /// <summary>
        /// Gets the users currently in the guild.
        /// </summary>
        [JsonPropertyName("members")]
        public List<GuildMember> Members { get; init; } = [];

        /// <summary>
        /// Gets the channels in the guild.
        /// </summary>
        [JsonPropertyName("channels")]
        [JsonInclude]
        internal List<Channel> Channels { get; set; } = [];

        /// <summary>
        /// Gets all active threads in the guild that the current user has permission to view.
        /// </summary>
        [JsonPropertyName("threads")]
        public Channel[] Threads { get; init; } = [];

        /// <summary>
        /// Gets the presence updates of the members in the guild.
        /// Will only include non-offline members if the size is greater than the large threshold.
        /// </summary>
        [JsonPropertyName("presences")]
        public List<Presence> Presences { get; init; } = [];

        /// <summary>
        /// Gets the stage instances in the guild.
        /// </summary>
        [JsonPropertyName("stage_instances")]
        public StageInstance[] StageInstances { get; init; } = [];

        /// <summary>
        /// Gets the scheduled events in the guild.
        /// </summary>
        [JsonPropertyName("guild_scheduled_events")]
        public GuildScheduledEvent[] GuildScheduledEvents { get; init; } = [];

        /// <summary>
        /// Gets the soundboard sounds in the guild.
        /// </summary>
        [JsonPropertyName("soundboard_sounds")]
        public SoundboardSound[] SoundboardSounds { get; internal set; } = [];

        #endregion

        #region Channels

        [JsonIgnore]
        /// <summary>
        /// A text channel within a server.
        /// TYPE: <see cref="ChannelType.GuildText"/>
        /// </summary>
        public Channel[] TextChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildText)];

        [JsonIgnore]
        /// <summary>
        /// A voice channel within a server.
        /// TYPE: <see cref="ChannelType.GuildVoice"/>
        /// </summary>
        public Channel[] VoiceChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildVoice)];

        [JsonIgnore]
        /// <summary>
        /// An organizational category that contains up to 50 channels.
        /// TYPE: <see cref="ChannelType.GuildCategory"/>
        /// </summary>
        public Channel[] Categories => [.. Channels.Where(x => x.Type == ChannelType.GuildCategory)];

        [JsonIgnore]
        /// <summary>
        /// A channel that users can follow and crosspost into their own server.
        /// TYPE: <see cref="ChannelType.GuildAnnouncement"/>
        /// </summary>
        public Channel[] AnnouncementChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildAnnouncement)];

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
        public Channel[] StageChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildStageVoice)];

        [JsonIgnore]
        /// <summary>
        /// The channel in a hub containing the listed servers.
        /// TYPE: <see cref="ChannelType.GuildDirectory"/>
        /// </summary>
        public Channel[] DirectoryChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildDirectory)];

        [JsonIgnore]
        /// <summary>
        /// A channel that can only contain threads.
        /// TYPE: <see cref="ChannelType.GuildForum"/>
        /// </summary>
        public Channel[] ForumChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildForum)];

        [JsonIgnore]
        /// <summary>
        /// A channel that can only contain threads, similar to GuildForum.
        /// TYPE: <see cref="ChannelType.GuildMedia"/>
        /// </summary>
        public Channel[] MediaChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildMedia)];
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

        public DiscordGuild()
        {
            SortVoiceStatesAccordingToChannel();
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

        internal bool CheckIfChannelIsVc(ulong channelId)
        {
            Channel? channel = GetChannel(channelId);
            return channel is not null && channel.Type == ChannelType.GuildVoice;
        }

        #region GetMethods

        public async Task<SoundboardSound[]> GetSoundboardSoundsAsync()
        {
            if (SoundboardSounds is null || SoundboardSounds.Length > 0)
            {
                SoundboardSounds = await DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>().GetSoundboardSoundsAsync(Id);
            }

            return SoundboardSounds;
        }

        public GuildMember? GetMember(ulong userId)
            => Members.FirstOrDefault(x => x.User?.Id == userId);

        public Channel? GetChannel(ulong channelId)
            => Channels.FirstOrDefault(x => x.Id == channelId);

        #endregion

        #region VoiceChannelHandling

        /// <summary>
        /// Connects the bot to a voice channel in a specific guild.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public async Task ConnectToVcAsync(ulong channelId, bool selfDeaf = false, bool selfMute = false)
        {
            Channel? channel = GetChannel(channelId);
            if (channel is null || channel.Type != ChannelType.GuildVoice)
            {
                throw new ArgumentException($"The channel either doesnt exist in this guild or is not a voice channel");
            }

            await DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>()
                .ConnectToVcAsync(Id, channelId, selfDeaf, selfMute);
        }

        /// <summary>
        /// Connects the bot to a voice channel in a specific guild.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public async Task ConnectToVcAsync(Channel channel, bool selfDeaf = false, bool selfMute = false)
        {
            if (channel.Type != ChannelType.GuildVoice || channel.GuildId != Id)
            {
                throw new ArgumentException($"The channel either doesnt exist in this guild or is not a voice channel");
            }

            await DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>()
                .ConnectToVcAsync(Id, channel.Id, selfDeaf, selfMute);
        }

        /// <summary>
        /// Disconnects the bot from the voice channel in a specific guild.
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectFromVcAsync()
           => await DiscordClient.ServiceProvider.GetRequiredService<DiscordClient>()
                .DisconnectFromVcAsync(Id);

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
            DiscordClient.Logger.LogInfo($"Adding channel: {channel.Name} to guild: {Name}");
            OnChannelCreated?.Invoke(this, channel);
            Channels.Add(channel);
        }

        internal void DeleteChannel(Channel channel)
        {
            DiscordClient.Logger.LogInfo($"Deleting channel: {channel.Name} from guild: {Name}");
            OnChannelDeleted?.Invoke(this, channel);
            Channels.RemoveAll(x => x.Id == channel.Id);
        }

        internal void UpdateChannel(Channel channel)
        {
            DiscordClient.Logger.LogInfo($"Updating channel: {channel.Name} from guild: {Name}");
            OnChannelUpdated?.Invoke(this, channel);

            int index = Channels.FindIndex(x => x.Id == channel.Id);
            Channels[index] = channel;
        }

        internal async Task<ChannelPins> UpdateChannelPinsAsync(ChannelPinsUpdate channelPinsUpdate)
        {
            Channel channel = GetChannel(channelPinsUpdate.ChannelId)!;
            channel.LastPinTimestamp = channelPinsUpdate.LastPinTimestamp;

            Message[] pinnedMessages = await channel.GetPinnedMessages() ?? [];
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
        internal async Task<Message[]?> GetPinnedMessages(ulong channelId)
        {
            Channel? channel = GetChannel(channelId);
            return channel is not null
                ? await channel.GetPinnedMessages()
                : null;
        }

        #endregion
    }
}
