namespace DiscordBotLibrary
{
    [Flags]
    public enum Intents
    {
        None = 0,

        // General events
        GUILDS = 1 << 0,                            
        GUILD_MEMBERS = 1 << 1,                     
        GUILD_BANS = 1 << 2,                        
        GUILD_EMOJIS_AND_STICKERS = 1 << 3,         
        GUILD_INTEGRATIONS = 1 << 4,                
        GUILD_WEBHOOKS = 1 << 5,                    
        GUILD_INVITES = 1 << 6,                     
        GUILD_VOICE_STATE = 1 << 7,                 
        GUILD_PRESENCES = 1 << 8,
        ALL_GUILD_EVENTS = GUILDS | GUILD_MEMBERS | GUILD_BANS | GUILD_EMOJIS_AND_STICKERS
            | GUILD_INTEGRATIONS | GUILD_WEBHOOKS | GUILD_INVITES | GUILD_VOICE_STATE | GUILD_PRESENCES,

        // Message events
        MESSAGE_CREATE = 1 << 9,                    
        MESSAGE_UPDATE = 1 << 10,                   
        MESSAGE_DELETE = 1 << 11,                   
        MESSAGE_REACTION_ADD = 1 << 12,             
        MESSAGE_REACTION_REMOVE = 1 << 13,          
        MESSAGE_REACTION_REMOVE_ALL = 1 << 14,      
        MESSAGE_REACTION_REMOVE_EMOJI = 1 << 15,    
        TYPING_START = 1 << 16,                     
        ALL_MESSAGE_EVENTS = MESSAGE_CREATE | MESSAGE_UPDATE | MESSAGE_DELETE | MESSAGE_REACTION_ADD 
            | MESSAGE_REACTION_REMOVE | MESSAGE_REACTION_REMOVE_ALL | MESSAGE_REACTION_REMOVE_EMOJI | TYPING_START,

        // Presence and user events
        PRESENCE_UPDATE = 1 << 17,                  
        USER_UPDATE = 1 << 18,    

        // Guild Scheduled Events
        GUILD_SCHEDULED_EVENTS = 1 << 19,           

        All = 1118481                               
    }
}
