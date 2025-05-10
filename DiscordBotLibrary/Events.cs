namespace DiscordBotLibrary
{
    /// <summary>
    /// An enumeration of all the events that can be received from the Discord API.
    /// </summary>
    internal enum Events : byte
    {
        // Connection Lifecycle
        READY = 0,
        RESUMED = 1,

        // Guild
        GUILD_CREATE = 2,
        GUILD_UPDATE = 3,
        GUILD_DELETE = 4,

        GUILD_ROLE_CREATE = 5,
        GUILD_ROLE_UPDATE = 6,
        GUILD_ROLE_DELETE = 7,

        GUILD_MEMBER_ADD = 8,
        GUILD_MEMBER_REMOVE = 9,
        GUILD_MEMBER_UPDATE = 10,
        GUILD_MEMBERS_CHUNK = 11,

        GUILD_BAN_ADD = 12,
        GUILD_BAN_REMOVE = 13,

        GUILD_EMOJIS_UPDATE = 14,
        GUILD_STICKERS_UPDATE = 15,

        GUILD_SCHEDULED_EVENT_CREATE = 16,
        GUILD_SCHEDULED_EVENT_UPDATE = 17,
        GUILD_SCHEDULED_EVENT_DELETE = 18,
        GUILD_SCHEDULED_EVENT_USER_ADD = 19,
        GUILD_SCHEDULED_EVENT_USER_REMOVE = 20,

        // Channel
        CHANNEL_CREATE = 21,
        CHANNEL_UPDATE = 22,
        CHANNEL_DELETE = 23,

        CHANNEL_PINS_UPDATE = 24,

        THREAD_CREATE = 25,
        THREAD_UPDATE = 26,
        THREAD_DELETE = 27,
        THREAD_LIST_SYNC = 28,
        THREAD_MEMBER_UPDATE = 29,
        THREAD_MEMBERS_UPDATE = 30,

        // Messages
        MESSAGE_CREATE = 31,
        MESSAGE_UPDATE = 32,
        MESSAGE_DELETE = 33,
        MESSAGE_DELETE_BULK = 34,

        MESSAGE_REACTION_ADD = 35,
        MESSAGE_REACTION_REMOVE = 36,
        MESSAGE_REACTION_REMOVE_ALL = 37,
        MESSAGE_REACTION_REMOVE_EMOJI = 38,

        TYPING_START = 39,

        // Voice
        VOICE_STATE_UPDATE = 40,
        VOICE_SERVER_UPDATE = 41,

        // Presence
        PRESENCE_UPDATE = 42,

        // Invites
        INVITE_CREATE = 43,
        INVITE_DELETE = 44,

        // Webhooks
        WEBHOOKS_UPDATE = 45,

        // Interactions
        INTERACTION_CREATE = 46,

        // Stage Instances
        STAGE_INSTANCE_CREATE = 47,
        STAGE_INSTANCE_UPDATE = 48,
        STAGE_INSTANCE_DELETE = 49
    }
}
