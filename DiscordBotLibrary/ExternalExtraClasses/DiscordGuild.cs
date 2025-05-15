namespace DiscordBotLibrary.ExternalExtraClasses
{
    /// <summary>
    /// Represents a Discord guild.
    /// If <c>Unavailable</c> is <c>true</c> most of the properties will be <c>null</c>.
    /// </summary>
    public record DiscordGuild : GuildCreateEventArgs
    {
        internal int ShardId { get; init; }

        #region Channels

        /// <summary>
        /// A text channel within a server.
        /// TYPE: <see cref="ChannelType.GuildText"/>
        /// </summary>
        public Channel[] TextChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildText)];

        /// <summary>
        /// A voice channel within a server.
        /// TYPE: <see cref="ChannelType.GuildVoice"/>
        /// </summary>
        public Channel[] VoiceChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildVoice)];

        /// <summary>
        /// An organizational category that contains up to 50 channels.
        /// TYPE: <see cref="ChannelType.GuildCategory"/>
        /// </summary>
        public Channel[] Categories => [.. Channels.Where(x => x.Type == ChannelType.GuildCategory)];

        /// <summary>
        /// A channel that users can follow and crosspost into their own server.
        /// TYPE: <see cref="ChannelType.GuildAnnouncement"/>
        /// </summary>
        public Channel[] AnnouncementChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildAnnouncement)];

        /// <summary>
        /// A temporary sub-channel within a GuildAnnouncement channel.
        /// TYPE: <see cref="ChannelType.AnnouncementThread"/>
        /// </summary>
        public Channel[] AnnouncementThreads => [.. Channels.Where(x => x.Type == ChannelType.AnnouncementThread)];

        /// <summary>
        /// A temporary sub-channel within a GuildText or GuildForum channel.
        /// TYPE: <see cref="ChannelType.PublicThread"/>
        /// </summary>
        public Channel[] PublicThreads => [.. Channels.Where(x => x.Type == ChannelType.PublicThread)];

        /// <summary>
        /// A temporary sub-channel within a GuildText channel with limited access.
        /// TYPE: <see cref="ChannelType.PrivateThread"/>
        /// </summary>
        public Channel[] PrivateThreads => [.. Channels.Where(x => x.Type == ChannelType.PrivateThread)];

        /// <summary>
        /// A voice channel for hosting events with an audience.
        /// TYPE: <see cref="ChannelType.GuildStageVoice"/>
        /// </summary>
        public Channel[] StageChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildStageVoice)];

        /// <summary>
        /// The channel in a hub containing the listed servers.
        /// TYPE: <see cref="ChannelType.GuildDirectory"/>
        /// </summary>
        public Channel[] DirectoryChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildDirectory)];

        /// <summary>
        /// A channel that can only contain threads.
        /// TYPE: <see cref="ChannelType.GuildForum"/>
        /// </summary>
        public Channel[] ForumChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildForum)];

        /// <summary>
        /// A channel that can only contain threads, similar to GuildForum.
        /// TYPE: <see cref="ChannelType.GuildMedia"/>
        /// </summary>
        public Channel[] MediaChannels => [.. Channels.Where(x => x.Type == ChannelType.GuildMedia)];
        #endregion

        #region Constructors

        public DiscordGuild(GuildCreateEventArgs guildCreateEventArgs, int shardId)
        {
            AfkChannelId = guildCreateEventArgs.AfkChannelId;
            AfkTimeout = guildCreateEventArgs.AfkTimeout;
            ApplicationId = guildCreateEventArgs.ApplicationId;
            ApproximateMemberCount = guildCreateEventArgs.ApproximateMemberCount;
            ApproximatePresenceCount = guildCreateEventArgs.ApproximatePresenceCount;
            Banner = guildCreateEventArgs.Banner;
            DefaultMessageNotifications = guildCreateEventArgs.DefaultMessageNotifications;
            Description = guildCreateEventArgs.Description;
            DiscoverySplash = guildCreateEventArgs.DiscoverySplash;
            Emojis = guildCreateEventArgs.Emojis;
            ExplicitContentFilter = guildCreateEventArgs.ExplicitContentFilter;
            Features = guildCreateEventArgs.Features;
            GuildScheduledEvents = guildCreateEventArgs.GuildScheduledEvents;
            Icon = guildCreateEventArgs.Icon;
            IconHash = guildCreateEventArgs.IconHash;
            Id = guildCreateEventArgs.Id;
            IncidentsData = guildCreateEventArgs.IncidentsData;
            JoinedAt = guildCreateEventArgs.JoinedAt;
            Large = guildCreateEventArgs.Large;
            MaxMembers = guildCreateEventArgs.MaxMembers;
            MaxPresences = guildCreateEventArgs.MaxPresences;
            MaxStageVideoChannelUsers = guildCreateEventArgs.MaxStageVideoChannelUsers;
            MaxVideoChannelUsers = guildCreateEventArgs.MaxVideoChannelUsers;
            MemberCount = guildCreateEventArgs.MemberCount;
            Members = guildCreateEventArgs.Members;
            MfaLevel = guildCreateEventArgs.MfaLevel;
            Name = guildCreateEventArgs.Name;
            NsfwLevel = guildCreateEventArgs.NsfwLevel;
            Owner = guildCreateEventArgs.Owner;
            OwnerId = guildCreateEventArgs.OwnerId;
            Permissions = guildCreateEventArgs.Permissions;
            PreferredLocale = guildCreateEventArgs.PreferredLocale;
            PremiumProgressBarEnabled = guildCreateEventArgs.PremiumProgressBarEnabled;
            PremiumSubscriptionCount = guildCreateEventArgs.PremiumSubscriptionCount;
            PremiumTier = guildCreateEventArgs.PremiumTier;
            Presences = guildCreateEventArgs.Presences;
            PublicUpdatesChannelId = guildCreateEventArgs.PublicUpdatesChannelId;
            Roles = guildCreateEventArgs.Roles;
            RulesChannelId = guildCreateEventArgs.RulesChannelId;
            SafetyAlertsChannelId = guildCreateEventArgs.SafetyAlertsChannelId;
            SoundboardSounds = guildCreateEventArgs.SoundboardSounds;
            Splash = guildCreateEventArgs.Splash;
            StageInstances = guildCreateEventArgs.StageInstances;
            Stickers = guildCreateEventArgs.Stickers;
            SystemChannelFlags = guildCreateEventArgs.SystemChannelFlags;
            SystemChannelId = guildCreateEventArgs.SystemChannelId;
            Unavailable = guildCreateEventArgs.Unavailable;
            VanityUrlCode = guildCreateEventArgs.VanityUrlCode;
            VerificationLevel = guildCreateEventArgs.VerificationLevel;
            WelcomeScreen = guildCreateEventArgs.WelcomeScreen;
            WidgetChannelId = guildCreateEventArgs.WidgetChannelId;
            WidgetEnabled = guildCreateEventArgs.WidgetEnabled;
            ShardId = shardId;
        }

        public DiscordGuild(UnavailableGuildCreateEventArgs unavailableGuildCreateEventArgs, int shardId)
        {
            JoinedAt = unavailableGuildCreateEventArgs.JoinedAt;
            GuildScheduledEvents = unavailableGuildCreateEventArgs.GuildScheduledEvents;
            Id = unavailableGuildCreateEventArgs.Id;
            Large = unavailableGuildCreateEventArgs.Large;
            MemberCount = unavailableGuildCreateEventArgs.MemberCount;
            Members = unavailableGuildCreateEventArgs.Members;
            Channels = unavailableGuildCreateEventArgs.Channels;
            Threads = unavailableGuildCreateEventArgs.Threads;
            Presences = unavailableGuildCreateEventArgs.Presences;
            VoiceStates = unavailableGuildCreateEventArgs.VoiceStates;
            StageInstances = unavailableGuildCreateEventArgs.StageInstances;
            SoundboardSounds = unavailableGuildCreateEventArgs.SoundboardSounds;
            Presences = unavailableGuildCreateEventArgs.Presences;
            Unavailable = unavailableGuildCreateEventArgs.Unavailable;
            ShardId = shardId;
        }

        #endregion
    }
}
