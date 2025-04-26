namespace DiscordBotLibrary
{
    internal class Guild
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Icon { get; set; }
        public string? IconHash { get; set; }
        public string? Splash { get; set; }
        public string? DiscoverySplash { get; set; }

        public bool? Owner { get; set; }
        public string OwnerId { get; set; } = default!;
        public string? Permissions { get; set; }
        public string? AfkChannelId { get; set; }
        public int AfkTimeout { get; set; }

        public bool? WidgetEnabled { get; set; }
        public string? WidgetChannelId { get; set; }

        public VerificationLevel VerificationLevel { get; set; }
        public NotificationLevel DefaultMessageNotifications { get; set; }
        public ExplicitContentFilterLevel ExplicitContentFilter { get; set; }

        public List<Role> Roles { get; set; } = new();
        public List<Emoji> Emojis { get; set; } = new();
        public List<string> Features { get; set; } = new();

        public MfaLevel MfaLevel { get; set; }
        public string? ApplicationId { get; set; }

        public string? SystemChannelId { get; set; }
        public SystemChannelFlags SystemChannelFlags { get; set; }
        public string? RulesChannelId { get; set; }

        public int? MaxPresences { get; set; }
        public int? MaxMembers { get; set; }

        public string? VanityUrlCode { get; set; }
        public string? Description { get; set; }
        public string? Banner { get; set; }

        public PremiumTier PremiumTier { get; set; }
        public int? PremiumSubscriptionCount { get; set; }

        public string? PreferredLocale { get; set; }
        public string? PublicUpdatesChannelId { get; set; }

        public int? MaxVideoChannelUsers { get; set; }
        public int? MaxStageVideoChannelUsers { get; set; }

        public int? ApproximateMemberCount { get; set; }
        public int? ApproximatePresenceCount { get; set; }

        public WelcomeScreen? WelcomeScreen { get; set; }
        public NsfwLevel NsfwLevel { get; set; }

        public List<Sticker>? Stickers { get; set; }
        public bool PremiumProgressBarEnabled { get; set; }
        public string? SafetyAlertsChannelId { get; set; }
        public IncidentsData? IncidentsData { get; set; }
    }

}
