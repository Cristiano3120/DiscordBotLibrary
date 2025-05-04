namespace DiscordBotLibrary
{
    [Flags]
    public enum SystemChannelFlags : byte
    {
        SuppressJoinNotifications = 1 << 0,
        SuppressPremiumSubscriptions = 1 << 1,
        SuppressGuildReminderNotifications = 1 << 2,
        SuppressJoinNotificationReplies = 1 << 3,
        SupressRoleSubscriptionPurchaseNotifications = 1 << 4,
        SuppressRoleSubscriptionPurchaseNotificationReplies = 1 << 5,
    }
}
