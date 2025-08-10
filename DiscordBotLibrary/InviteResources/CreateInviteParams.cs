namespace DiscordBotLibrary.InviteResources
{
    public sealed record CreateInviteParams
    {
        /// <summary>
        /// duration of invite in seconds before expiry, or 0 for never. between 0 and 604800 (7 days) <br></br>
        /// DEFAULT: 86400 (24 hours)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<uint> MaxAge { get; set; }

        /// <summary>
        /// max number of uses or 0 for unlimited. between 0 and 100
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<uint> MaxUses { get; set; }

        /// <summary>
        /// whether this invite only grants temporary membership false
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Temporary { get; set; }

        /// <summary>
        /// if true, don't try to reuse a similar invite (useful for creating many unique one time use invites) false
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]

        public Optional<bool> Unique { get; set; }

        /// <summary>
        /// the type of target for this voice channel invite
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<InviteTargetType> TargetType { get; set; }

        /// <summary>
        /// the id of the user whose stream to display for this invite, required if target_type is 1, the user must be streaming in the channel
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ulong> TargetUserId { get; set; }

        /// <summary>
        /// the id of the embedded application to open for this invite, required if target_type is 2, the application must have the EMBEDDED flag
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ulong> TargetApplicationId { get; set; }
    }
}
