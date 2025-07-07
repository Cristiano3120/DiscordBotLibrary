namespace DiscordBotLibrary.ChannelResources
{
    internal sealed record VoiceRegion
    {
        /// <summary>
        /// unique ID for the region
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; init; }

        /// <summary>
        /// name of the region
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; }

        /// <summary>
        /// true for a single server that is closest to the current user's client
        /// </summary>
        [JsonProperty("optimal")]
        public bool Optimal { get; init; }

        /// <summary>
        /// whether this is a deprecated voice region (avoid switching to these)
        /// </summary>
        [JsonProperty("deprecated")]
        public bool Deprecated { get; init; }

        /// <summary>
        /// whether this is a custom voice region (used for events/etc)
        /// </summary>
        [JsonProperty("custom")]
        public bool Custom { get; init; }
    }
}
