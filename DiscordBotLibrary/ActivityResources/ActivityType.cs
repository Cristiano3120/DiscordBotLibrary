namespace DiscordBotLibrary.ActivityResources
{
    /// <summary>
    /// Represents the type of an activity in Discord Rich Presence.
    /// </summary>
    public enum ActivityType : byte
    {
        /// <summary>
        /// Playing {name}.  
        /// Example: "Playing Rocket League"
        /// </summary>
        Playing = 0,

        /// <summary>
        /// Streaming {details}.  
        /// Example: "Streaming Rocket League"
        /// </summary>
        Streaming = 1,

        /// <summary>
        /// Listening to {name}.  
        /// Example: "Listening to Spotify"
        /// </summary>
        Listening = 2,

        /// <summary>
        /// Watching {name}.  
        /// Example: "Watching YouTube Together"
        /// </summary>
        Watching = 3,

        /// <summary>
        /// Custom status: {emoji} {state}.  
        /// Example: ":smiley: I am cool"
        /// </summary>
        Custom = 4,

        /// <summary>
        /// Competing in {name}.  
        /// Example: "Competing in Arena World Champions"
        /// </summary>
        Competing = 5
    }

}