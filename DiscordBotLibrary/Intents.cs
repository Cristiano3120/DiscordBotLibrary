namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents the Gateway Intents used to specify which events the bot will receive.
    /// </summary>
    [Flags]
    public enum Intents
    {
        None = 0,

        // General events
        Guilds = 1 << 0,
        GuildMembers = 1 << 1,
        GuildBans = 1 << 2,
        GuildEmojisAndStickers = 1 << 3,
        GuildIntegrations = 1 << 4,
        GuildWebhooks = 1 << 5,
        GuildInvites = 1 << 6,
        GuildVoiceState = 1 << 7,
        GuildPresences = 1 << 8,
        AllGuildEvents = Guilds | GuildMembers | GuildBans | GuildEmojisAndStickers
            | GuildIntegrations | GuildWebhooks | GuildInvites | GuildVoiceState | GuildPresences,

        // Message events
        MessageCreate = 1 << 9,
        MessageUpdate = 1 << 10,
        MessageDelete = 1 << 11,
        MessageReactionAdd = 1 << 12,
        MessageReactionRemove = 1 << 13,
        MessageReactionRemoveAll = 1 << 14,
        MessageReactionRemoveEmoji = 1 << 15,
        TypingStart = 1 << 16,
        AllMessageEvents = MessageCreate | MessageUpdate | MessageDelete | MessageReactionAdd
            | MessageReactionRemove | MessageReactionRemoveAll | MessageReactionRemoveEmoji | TypingStart,

        // Presence and user events
        PresenceUpdate = 1 << 17,
        UserUpdate = 1 << 18,

        // Guild Scheduled Events
        GuildScheduledEvents = 1 << 19,

        All = 1118481
    }

}
