using DiscordBotLibrary.GuildResources.GuildScheduledEventResources;

namespace DiscordBotLibrary.InviteResources
{
    public record InviteData
    {
        /// <summary>
        /// the type of invite
        /// </summary>
        public InviteType Type { get; init; }

        /// <summary>
        /// the invite code (unique ID)
        /// </summary>
        public string Code { get; init; } = default!;

        /// <summary>
        /// the guild this invite is for [partial obj only fields like ID are filled] (Null if InviteType != Guild)
        /// </summary>
        public Guild? Guild { get; init; }

        /// <summary>
        /// the channel this invite is for
        /// </summary>
        public Channel? Channel { get; init; }

        /// <summary>
        /// the user who created the invite
        /// </summary>
        public User? Inviter { get; init; }

        /// <summary>
        /// the type of target for this voice channel invite
        /// </summary>
        public int? TargetType { get; init; }

        /// <summary>
        /// the user whose stream to display for this voice channel stream invite
        /// </summary>
        public User? TargetUser { get; init; }

        /// <summary>
        /// the embedded application to open for this voice channel embedded application invite
        /// </summary>
        public Application? TargetApplication { get; init; }

        /// <summary>
        /// approximate count of online members, returned from the GET /invites/<code> endpoint when with_counts is true
        /// </summary>
        public int? ApproximatePresenceCount { get; init; }

        /// <summary>
        /// approximate count of total members, returned from the GET /invites/<code> endpoint when with_counts is true
        /// </summary>
        public int? ApproximateMemberCount { get; init; }

        /// <summary>
        /// the expiration date of this invite, returned from the GET /invites/<code> endpoint when with_expiration is true
        /// </summary>
        public DateTimeOffset? ExpiresAt { get; init; }

        /// <summary>
        /// stage instance data if there is a public Stage instance in the Stage channel this invite is for (deprecated)
        /// </summary>
        public InviteStageInstance? StageInstance { get; init; }

        /// <summary>
        /// guild scheduled event data, only included if guild_scheduled_event_id contains a valid guild scheduled event id
        /// </summary>
        public GuildScheduledEvent? GuildScheduledEvent { get; init; }

        /// <summary>
        /// guild invite flags for guild invites
        /// </summary>
        public InviteFlags? Flags { get; init; }

        internal InviteData() { }
    }
}
