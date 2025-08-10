namespace DiscordBotLibrary.ChannelResources
{
    public sealed record ArchivedThreadsResponse
    {
        /// <summary>
        /// The archived threads
        /// </summary>
        public required Channel[] Threads { get; init; }

        /// <summary>
        /// A thread member object for each returned thread the current user has joined
        /// </summary>
        public required ThreadMember[] Members { get; init; }

        /// <summary>
        /// Whether there are potentially additional threads that could be returned on a subsequent call
        /// </summary>
        public bool HasMore { get; init; }

        private ArchivedThreadsResponse() { }
    }
}
