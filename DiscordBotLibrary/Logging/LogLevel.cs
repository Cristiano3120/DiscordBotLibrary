namespace DiscordBotLibrary.Logging
{
    /// <summary>
    /// Log levels for the Discord bot library. This is used to control the amount of logging that is done by the library.
    /// </summary>
    public enum LogLevel : byte
    {
        /// <summary>
        /// No logging. Keeps the Console clean
        /// </summary>
        None = 0,
        /// <summary>
        /// Only errors
        /// </summary>
        Error = 1,
        /// <summary>
        /// Warnings and errors
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Information, warnings and errors. This is the default level
        /// </summary>
        Info = 3,
        /// <summary>
        /// Logs everything
        /// </summary>
        Debug = 4,
    }
}
