namespace DiscordBotLibrary.ChannelResources
{
    internal sealed record VoiceRegion
    {
        /// <summary>
        /// unique ID for the region
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; init; }

        /// <summary>
        /// name of the region
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; }

        /// <summary>
        /// true for a single server that is closest to the current user's client
        /// </summary>
        [JsonPropertyName("optimal")]
        public bool Optimal { get; init; }

        /// <summary>
        /// whether this is a deprecated voice region (avoid switching to these)
        /// </summary>
        [JsonPropertyName("deprecated")]
        public bool Deprecated { get; init; }

        /// <summary>
        /// whether this is a custom voice region (used for events/etc)
        /// </summary>
        [JsonPropertyName("custom")]
        public bool Custom { get; init; }
    }
}
