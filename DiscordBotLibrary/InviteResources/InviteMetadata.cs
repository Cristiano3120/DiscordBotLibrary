namespace DiscordBotLibrary.InviteResources
{
    public sealed record GuildInvite : InviteData
    {
        /// <summary>
        /// number of times this invite has been used
        /// </summary>
        public int Uses { get; init; }

        /// <summary>
        /// max number of times this invite can be used
        /// </summary>
        public int MaxUses { get; init; }

        /// <summary>
        /// duration (in seconds) after which the invite expires
        /// </summary>
        [JsonProperty]
        internal int MaxAge { get; init; }

        /// <summary>
        /// whether this invite only grants temporary membership
        /// </summary>
        public bool Temporary { get; init; }

        /// <summary>
        /// when this invite was created
        /// </summary>
        public DateTimeOffset CreatedAt { get; init; }

        private GuildInvite() { }
    }
}
