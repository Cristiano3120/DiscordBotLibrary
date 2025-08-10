namespace DiscordBotLibrary.InviteResources
{
    public class InviteStageInstance
    {
        /// <summary>
        /// the members speaking in the Stage
        /// </summary>
        public GuildMember[] Members { get; init; } = default!;

        /// <summary>
        /// the number of users in the Stage
        /// </summary>
        public int ParticipantCount { get; init; }

        /// <summary>
        /// the number of users speaking in the Stage
        /// </summary>
        public int SpeakerCount { get; init; }

        /// <summary>
        /// the topic of the Stage instance (1-120 characters)
        /// </summary>
        public string Topic { get; init; } = default!;
    }
}