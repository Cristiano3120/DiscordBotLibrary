namespace DiscordBotLibrary.ThreadMemberResources
{
    public sealed record ThreadMember
    {
        /// <summary>
        /// omitted on the member sent within each thread in the GUILD_CREATE event.
        /// </summary>
        [JsonProperty("id")]
        public ulong? ThreadId { get; init; }

        /// <summary>
        /// omitted on the member sent within each thread in the GUILD_CREATE event.
        /// </summary>
        public ulong? UserId { get; init; }

        public DateTime JoinTimestamp { get; init; }

        public ThreadMemberFlags Flags { get; init; }

        /// <summary>
        /// Only present when with_member is set to true when calling List Thread Members or Get Thread Member.
        /// </summary>
        public GuildMember? Member { get; init; }
    }
}