namespace DiscordBotLibrary
{
    public enum LogLevel
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
