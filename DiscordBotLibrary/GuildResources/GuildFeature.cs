using System.Runtime.Serialization;

namespace DiscordBotLibrary.GuildResources
{
    /// <summary>
    /// Features that a guild may have, based on Discord API's "guild features".
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GuildFeature : byte
    {
        [EnumMember(Value = "ANIMATED_BANNER")]
        AnimatedBanner,

        [EnumMember(Value = "ANIMATED_ICON")]
        AnimatedIcon,

        [EnumMember(Value = "APPLICATION_COMMAND_PERMISSIONS_V2")]
        ApplicationCommandPermissionsV2,

        [EnumMember(Value = "AUTO_MODERATION")]
        AutoModeration,

        [EnumMember(Value = "BANNER")]
        Banner,

        [EnumMember(Value = "COMMUNITY")]
        Community,

        [EnumMember(Value = "CREATOR_MONETIZABLE_PROVISIONAL")]
        CreatorMonetizableProvisional,

        [EnumMember(Value = "CREATOR_STORE_PAGE")]
        CreatorStorePage,

        [EnumMember(Value = "DEVELOPER_SUPPORT_SERVER")]
        DeveloperSupportServer,

        [EnumMember(Value = "DISCOVERABLE")]
        Discoverable,

        [EnumMember(Value = "ENHANCED_ROLE_COLORS")]
        EnhancedRoleColors,

        [EnumMember(Value = "FEATURABLE")]
        Featurable,

        [EnumMember(Value = "HAS_DIRECTORY_ENTRY")]
        HasDirectoryEntry,

        [EnumMember(Value = "HUB")]
        Hub,

        [EnumMember(Value = "INVITES_DISABLED")]
        InvitesDisabled,

        [EnumMember(Value = "INVITE_SPLASH")]
        InviteSplash,

        [EnumMember(Value = "LINKED_TO_HUB")]
        LinkedToHub,

        [EnumMember(Value = "MEMBER_VERIFICATION_GATE_ENABLED")]
        MemberVerificationGateEnabled,

        [EnumMember(Value = "MONETIZATION_ENABLED")]
        MonetizationEnabled,

        [EnumMember(Value = "MORE_SOUNDBOARD")]
        MoreSoundboard,

        [EnumMember(Value = "MORE_STICKERS")]
        MoreStickers,

        [EnumMember(Value = "NEWS")]
        News,

        [EnumMember(Value = "PARTNERED")]
        Partnered,

        [EnumMember(Value = "PREVIEW_ENABLED")]
        PreviewEnabled,

        [EnumMember(Value = "PRIVATE_THREADS")]
        PrivateThreads,

        [EnumMember(Value = "RAID_ALERTS_DISABLED")]
        RaidAlertsDisabled,

        [EnumMember(Value = "RELAY_ENABLED")]
        RelayEnabled,
        [EnumMember(Value = "ROLE_ICONS")]
        RoleIcons,

        [EnumMember(Value = "ROLE_SUBSCRIPTIONS_AVAILABLE_FOR_PURCHASE")]
        RoleSubscriptionsAvailableForPurchase,

        [EnumMember(Value = "ROLE_SUBSCRIPTIONS_ENABLED")]
        RoleSubscriptionsEnabled,

        [EnumMember(Value = "SOUNDBOARD")]
        Soundboard,

        [EnumMember(Value = "TICKETED_EVENTS_ENABLED")]
        TicketedEventsEnabled,

        [EnumMember(Value = "VANITY_URL")]
        VanityUrl,

        [EnumMember(Value = "VERIFIED")]
        Verified,

        [EnumMember(Value = "VIP_REGIONS")]
        VIPRegions,

        [EnumMember(Value = "WELCOME_SCREEN_ENABLED")]
        WelcomeScreenEnabled,

        [EnumMember(Value = "AUDIO_BITRATE_128_KBPS")]
        AudioBitrate128Kbps,

        [EnumMember(Value = "GUILD_TAGS")]
        GuildTags,

        [EnumMember(Value = "STAGE_CHANNEL_VIEWERS_50")]
        StageChannelViewers50,

        [EnumMember(Value = "VIDEO_QUALITY_720_60FPS")]
        VideoQuality72060Fps,

        [EnumMember(Value = "VIDEO_BITRATE_ENHANCED")]
        VideoBitrateEnhanced,

        [EnumMember(Value = "ENABLED_MODERATION_EXPERIENCE_FOR_NON_COMMUNITY")]
        EnabledModerationExperienceForNonCommunity,

        /// <summary>Guild has access to tierless boosting (no slot limits or required boost count).</summary>
        [EnumMember(Value = "TIERLESS_BOOSTING")]
        TierlessBoosting,

        /// <summary>Guild shows a system message when someone boosts, even if tierless.</summary>
        [EnumMember(Value = "TIERLESS_BOOSTING_SYSTEM_MESSAGE")]
        TierlessBoostingSystemMessage,
    }
}