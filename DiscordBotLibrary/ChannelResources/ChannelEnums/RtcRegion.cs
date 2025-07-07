using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace DiscordBotLibrary.ChannelResources.ChannelEnums
{
    /// <summary>
    /// The possible server locations a <see cref="Channel"/> of type <see cref="ChannelType.Voice"/> can have
    /// </summary>

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RtcRegion : byte
    {
        Automatic,

        [EnumMember(Value = "us-west")]
        UsWest,

        [EnumMember(Value = "us-east")]
        UsEast,

        [EnumMember(Value = "us-central")]
        UsCentral,

        [EnumMember(Value = "us-south")]
        UsSouth,

        [EnumMember(Value = "singapore")]
        Singapore,

        [EnumMember(Value = "japan")]
        Japan,

        [EnumMember(Value = "hongkong")]
        HongKong,

        [EnumMember(Value = "brazil")]
        Brazil,

        [EnumMember(Value = "sydney")]
        Sydney,

        [EnumMember(Value = "southafrica")]
        SouthAfrica,

        [EnumMember(Value = "india")]
        India,

        [EnumMember(Value = "rotterdam")]
        Rotterdam
    }
}
