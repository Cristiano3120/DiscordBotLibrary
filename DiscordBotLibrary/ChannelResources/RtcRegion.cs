namespace DiscordBotLibrary.ChannelResources
{
    /// <summary>
    /// The possible server locations a <see cref="Channel"/> of type <see cref="ChannelType.Voice"/> can have
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RtcRegion : byte
    {
        Automatic,

        [JsonStringEnumMemberName("us-west")]
        UsWest,

        [JsonStringEnumMemberName("us-east")]
        UsEast,

        [JsonStringEnumMemberName("us-central")]
        UsCentral,

        [JsonStringEnumMemberName("us-south")]
        UsSouth,

        [JsonStringEnumMemberName("singapore")]
        Singapore,

        [JsonStringEnumMemberName("japan")]
        Japan,

        [JsonStringEnumMemberName("hongkong")]
        HongKong,

        [JsonStringEnumMemberName("brazil")]
        Brazil,

        [JsonStringEnumMemberName("sydney")]
        Sydney,

        [JsonStringEnumMemberName("southafrica")]
        SouthAfrica,

        [JsonStringEnumMemberName("india")]
        India,

        [JsonStringEnumMemberName("rotterdam")]
        Rotterdam
    }
}
