namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public class BaseChannelEdit
    {
        /// <summary>
        /// For some channel types this gotta be in a specific format
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// the position of the channel in the left-hand listing (channels with the same position are sorted by id)	
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<uint> Position { get; set; }

        /// <summary>
        /// channel or category-specific permissions
        /// </summary>
        [JsonProperty("permission_overwrites", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<Overwrite> PermissionOverwrites { get; set; }

        internal BaseChannelEdit() { }
    }
}
