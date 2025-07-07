using System.Runtime.Serialization;

namespace DiscordBotLibrary.GuildResources
{
    /// <summary>
    /// Features that a guild may have, based on Discord API's "guild features".
    /// </summary>
    public enum GuildFeature
    {
        /// <summary>Guild has access to set an animated guild banner image.</summary>
        [EnumMember(Value = "ANIMATED_BANNER")]
        AnimatedBanner,

        /// <summary>Guild has access to set an animated guild icon.</summary>
        [EnumMember(Value = "ANIMATED_ICON")]
        AnimatedIcon,

        /// <summary>Guild uses the old permissions configuration behavior for application commands.</summary>
        [EnumMember(Value = "APPLICATION_COMMAND_PERMISSIONS_V2")]
        ApplicationCommandPermissionsV2,

        /// <summary>Guild has set up auto moderation rules.</summary>
        [EnumMember(Value = "AUTO_MODERATION")]
        AutoModeration,

        /// <summary>Guild has access to set a guild banner image.</summary>
        [EnumMember(Value = "BANNER")]
        Banner,

        /// <summary>Guild has access to community features such as welcome screen, stage channels, discovery, and more.</summary>
        [EnumMember(Value = "COMMUNITY")]
        Community,

        /// <summary>Guild has enabled monetization (creator monetizable provisional).</summary>
        [EnumMember(Value = "CREATOR_MONETIZABLE_PROVISIONAL")]
        CreatorMonetizableProvisional,

        /// <summary>Guild has enabled the role subscription promo store page.</summary>
        [EnumMember(Value = "CREATOR_STORE_PAGE")]
        CreatorStorePage,

        /// <summary>Guild is marked as a developer support server in the App Directory.</summary>
        [EnumMember(Value = "DEVELOPER_SUPPORT_SERVER")]
        DeveloperSupportServer,

        /// <summary>Guild is discoverable in the directory.</summary>
        [EnumMember(Value = "DISCOVERABLE")]
        Discoverable,

        /// <summary>Guild can be featured in the directory.</summary>
        [EnumMember(Value = "FEATURABLE")]
        Featurable,

        /// <summary>Guild has paused invites (no new users can join).</summary>
        [EnumMember(Value = "INVITES_DISABLED")]
        InvitesDisabled,

        /// <summary>Guild can set an invite splash background.</summary>
        [EnumMember(Value = "INVITE_SPLASH")]
        InviteSplash,

        /// <summary>Guild has enabled Membership Screening (verification gate).</summary>
        [EnumMember(Value = "MEMBER_VERIFICATION_GATE_ENABLED")]
        MemberVerificationGateEnabled,

        /// <summary>Guild has increased the number of soundboard sound slots.</summary>
        [EnumMember(Value = "MORE_SOUNDBOARD")]
        MoreSoundboard,

        /// <summary>Guild has increased the number of custom sticker slots.</summary>
        [EnumMember(Value = "MORE_STICKERS")]
        MoreStickers,

        /// <summary>Guild has access to create announcement/news channels.</summary>
        [EnumMember(Value = "NEWS")]
        News,

        /// <summary>Guild is a Discord partner.</summary>
        [EnumMember(Value = "PARTNERED")]
        Partnered,

        /// <summary>Guild allows previews before joining via Membership Screening or directory.</summary>
        [EnumMember(Value = "PREVIEW_ENABLED")]
        PreviewEnabled,

        /// <summary>Guild has disabled alerts for join raids.</summary>
        [EnumMember(Value = "RAID_ALERTS_DISABLED")]
        RaidAlertsDisabled,

        /// <summary>Guild is able to set role icons.</summary>
        [EnumMember(Value = "ROLE_ICONS")]
        RoleIcons,

        /// <summary>Guild has enabled role subscriptions that can be purchased.</summary>
        [EnumMember(Value = "ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE")]
        RoleSubscriptionsAvailableForPurchase,

        /// <summary>Guild has enabled role subscriptions.</summary>
        [EnumMember(Value = "ROLE_SUBSCRIPTIONS_ENABLED")]
        RoleSubscriptionsEnabled,

        /// <summary>Guild has access to create and use soundboard sounds.</summary>
        [EnumMember(Value = "SOUNDBOARD")]
        Soundboard,

        /// <summary>Guild has enabled ticketed events.</summary>
        [EnumMember(Value = "TICKETED_EVENTS_ENABLED")]
        TicketedEventsEnabled,

        /// <summary>Guild has access to a custom vanity URL (e.g., discord.gg/myserver).</summary>
        [EnumMember(Value = "VANITY_URL")]
        VanityUrl,

        /// <summary>Guild is verified by Discord.</summary>
        [EnumMember(Value = "VERIFIED")]
        Verified,

        /// <summary>Guild has access to set 384kbps voice bitrate (formerly VIP servers).</summary>
        [EnumMember(Value = "VIP_REGIONS")]
        VipRegions,

        /// <summary>Guild has enabled the welcome screen feature.</summary>
        [EnumMember(Value = "WELCOME_SCREEN_ENABLED")]
        WelcomeScreenEnabled,

        /// <summary>Guild can issue guest invites.</summary>
        [EnumMember(Value = "GUESTS_ENABLED")]
        GuestsEnabled,

        /// <summary>Guild can use gradient (enhanced) role colors.</summary>
        [EnumMember(Value = "ENHANCED_ROLE_COLORS")]
        EnhancedRoleColors
    }
}