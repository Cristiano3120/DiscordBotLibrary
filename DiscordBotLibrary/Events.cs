namespace DiscordBotLibrary
{
    internal enum Events : byte
    {
        // Connection Lifecycle
        READY,
        RESUMED,

        // Guild
        GUILD_CREATE,
        GUILD_UPDATE,
        GUILD_DELETE,

        GUILD_ROLE_CREATE,
        GUILD_ROLE_UPDATE,
        GUILD_ROLE_DELETE,

        GUILD_MEMBER_ADD,
        GUILD_MEMBER_REMOVE,
        GUILD_MEMBER_UPDATE,
        GUILD_MEMBERS_CHUNK,

        GUILD_BAN_ADD,
        GUILD_BAN_REMOVE,

        GUILD_EMOJIS_UPDATE,
        GUILD_STICKERS_UPDATE,

        GUILD_SCHEDULED_EVENT_CREATE,
        GUILD_SCHEDULED_EVENT_UPDATE,
        GUILD_SCHEDULED_EVENT_DELETE,
        GUILD_SCHEDULED_EVENT_USER_ADD,
        GUILD_SCHEDULED_EVENT_USER_REMOVE,

        // Channel
        CHANNEL_CREATE,
        CHANNEL_UPDATE,
        CHANNEL_DELETE,

        CHANNEL_PINS_UPDATE,

        THREAD_CREATE,
        THREAD_UPDATE,
        THREAD_DELETE,
        THREAD_LIST_SYNC,
        THREAD_MEMBER_UPDATE,
        THREAD_MEMBERS_UPDATE,

        // Messages
        MESSAGE_CREATE,
        MESSAGE_UPDATE,
        MESSAGE_DELETE,
        MESSAGE_DELETE_BULK,

        MESSAGE_REACTION_ADD,
        MESSAGE_REACTION_REMOVE,
        MESSAGE_REACTION_REMOVE_ALL,
        MESSAGE_REACTION_REMOVE_EMOJI,

        TYPING_START,

        // Voice
        VOICE_STATE_UPDATE,
        VOICE_SERVER_UPDATE,

        // Presence
        PRESENCE_UPDATE,

        // Invites
        INVITE_CREATE,
        INVITE_DELETE,

        // Webhooks
        WEBHOOKS_UPDATE,

        // Interactions
        INTERACTION_CREATE,

        // Stage Instances
        STAGE_INSTANCE_CREATE,
        STAGE_INSTANCE_UPDATE,
        STAGE_INSTANCE_DELETE
    }
}
