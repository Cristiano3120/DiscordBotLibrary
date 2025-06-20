namespace DiscordBotLibrary.Exceptions
{
    internal class MissingIntentException : Exception
    {
        public Intents[] RequiredIntents { get; private set; } = [];
        public Intents[] MissingIntents { get; private set; } = [];
        public string CallerName { get; private set; }
        public int LineNumber { get; private set; }

        public MissingIntentException(Intents[] requiredIntents, HashSet<Intents> missingIntents, CallerInfos callerInfos) 
            : base($"The method[{callerInfos.CallerName}] in {callerInfos.FilePath} at line {callerInfos.LineNum} needs the following Intents: {string.Join(", ", requiredIntents)} \n " +
                  $"You need to activate the following intents to be able to run this method: {string.Join(", ", missingIntents)}")
        {
            Source = callerInfos.FilePath;
            RequiredIntents = requiredIntents;
            MissingIntents = [..missingIntents];
            CallerName = callerInfos.CallerName;
            LineNumber = callerInfos.LineNum;
        }
    }
}
