using System.Runtime.Serialization;

namespace DiscordBotLibrary.EmbedResources
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmbedType : byte
    {
        [EnumMember(Value = "rich")]
        Rich,

        [EnumMember(Value = "image")]
        Image,

        [EnumMember(Value = "video")]
        Video,

        [EnumMember(Value = "gifv")]
        Gifv,

        [EnumMember(Value = "article")]
        Article,

        [EnumMember(Value = "link")]
        Link,

        [EnumMember(Value = "poll_result")]
        Poll_result
    }
}