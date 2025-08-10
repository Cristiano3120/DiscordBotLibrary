namespace DiscordBotLibrary.GuildResources
{
    internal sealed record ModifyMember
    {
        /// <summary>
        /// value to set user's nickname to     
        /// <br></br> Requires: MANAGE_NICKNAMES
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<string> Nick { get; init; }

        /// <summary>
        /// array of role ids the member is assigned    
        /// <br></br> Requires: MANAGE_ROLES
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ulong[]> Roles { get; init; }

        /// <summary>
        /// whether the user is muted in voice channels. Will throw a 400 error if the user is not in a voice channel  
        /// <br></br> Requires: MUTE_MEMBERS
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Mute { get; init; }

        /// <summary>
        /// whether the user is deafened in voice channels. Will throw a 400 error if the user is not in a voice channel
        /// <br></br> Requires: DEAFEN_MEMBERS
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<bool> Deaf { get; init; }

        /// <summary>
        /// id of channel to move user to (if they are connected to voice)      
        /// <br></br> Requires: MOVE_MEMBERS
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<ulong?> ChannelId { get; init; }

        /// <summary>
        /// when the user's timeout will expire and the user will be able to communicate in the guild again (up to 28 days in the future), set to null to remove timeout. Will throw a 403 error if the user has the ADMINISTRATOR permission or is the owner of the guild      
        /// <br></br> Requires: MODERATE_MEMBERS
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<DateTimeOffset> CommunicationDisabledUntil { get; init; }

        /// <summary>
        /// guild member flags  
        /// <br></br> Requires: MANAGE_GUILD or MANAGE_ROLES or (MODERATE_MEMBERS and KICK_MEMBERS and BAN_MEMBERS)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<GuildMemberFlags> Flags { get; init; }
    }
}
