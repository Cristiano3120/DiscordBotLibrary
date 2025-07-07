namespace DiscordBotLibrary.ChannelResources.ChannelEditResources
{
    public sealed class GroupDmEdit
    {
        /// <summary>
        /// 1-100 character channel name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// icon: binary as base64 encoded 
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Optional<string> Icon { get; set; }

        internal GroupDmEdit(string name)
        {
            Name = name;
        }

        private GroupDmEdit() { }
    }
}
