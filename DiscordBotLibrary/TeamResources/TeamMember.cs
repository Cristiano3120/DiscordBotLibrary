namespace DiscordBotLibrary.TeamResources
{
    /// <summary>
    /// Represents a member of a team.
    /// </summary>
    public sealed record TeamMember
    {
        /// <summary>
        /// Gets the user's membership state on the team.
        /// </summary>
        [JsonPropertyName("membership_state")]
        public TeamMembershipState MembershipState { get; init; }

        /// <summary>
        /// Gets the unique ID of the parent team of which the user is a member.
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("team_id")]
        public string TeamId { get; init; } = default!;

        /// <summary>
        /// Gets the partial user object representing the user, including avatar, discriminator, ID, and username.
        /// </summary>
        [JsonPropertyName("user")]
        public User User { get; init; } = default!;

        /// <summary>
        /// Gets the role of the team member.
        /// </summary>
        [JsonPropertyName("role")]
        public TeamMemberRole Role { get; init; } = default!;
    }

}