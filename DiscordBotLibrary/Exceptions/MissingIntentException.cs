namespace DiscordBotLibrary.Exceptions
{
    internal class MissingIntentException : Exception
    {
        public Intents[] RequiredIntents { get; private set; } = [];
        public Intents[] MissingIntents { get; private set; } = [];

        public MissingIntentException(Intents[] requiredIntents, HashSet<Intents> missingIntents, string methodSignature) 
            : base($"The method[{methodSignature}] needs the following Intents: {string.Join(", ", requiredIntents)} \n " +
                  $"You need to activate the following intents to be able to run this method: {string.Join(", ", missingIntents)}")
        {
            Source = methodSignature;
            RequiredIntents = requiredIntents;
            MissingIntents = [..missingIntents];
        }
    }
}
