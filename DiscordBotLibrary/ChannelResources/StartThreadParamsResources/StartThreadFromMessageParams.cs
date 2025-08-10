namespace DiscordBotLibrary.ChannelResources.StartThreadParamsResources
{
    /// <summary>
    /// Represents the parameters required to start a new thread from a message.
    /// </summary>
    public record StartThreadFromMessageParams
    {
        /// <summary>
        /// 1-100 character channel name
        /// </summary>
        public required string Name
        {
            get; 
            init
            {
                field = value;
                if (string.IsNullOrEmpty(field) || field.Length > 100)
                {
                    throw new ArgumentException("Thread name must be between 1 and 100 characters long.", nameof(Name));
                }
            }
        }

        /// <summary>
        /// The thread will stop showing in the channel list after auto_archive_duration minutes of inactivity
        /// </summary>
        public AutoArchiveDuration? AutoArchiveDuration { get; set; }

        /// <summary>
        /// Amount of seconds a user has to wait before sending another message
        /// </summary>
        [JsonProperty("rate_limit_per_user")]
        public Slowmode? Slowmode { get; set; }
    }
}
