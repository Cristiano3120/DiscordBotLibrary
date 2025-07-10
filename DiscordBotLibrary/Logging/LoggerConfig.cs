namespace DiscordBotLibrary.Logging
{
    public readonly struct LoggerConfig(byte maxAmmountOfLoggingFiles = 10, string pathToLoggingFolder = @"Logs/"
            , LogLevel logLevel = LogLevel.Info)
    {
        public byte MaxAmmountOfLoggingFiles { get; init; } = maxAmmountOfLoggingFiles;

        /// <summary> If you dont give this a value, it will just create a folder named "Logging" in the current directory.</summary>
        public string PathToLoggingFolder { get; init; } = pathToLoggingFolder;
        public LogLevel LogLevel { get; init; } = logLevel;

        public LoggerConfig() : this(10, @"Logs/", LogLevel.Info) { }
    }
}
