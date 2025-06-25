namespace DiscordBotLibrary.EmbedResources
{
    public enum EmbedType : byte
    {
        [JsonStringEnumMemberName("rich")]
        Rich,

        [JsonStringEnumMemberName("image")]
        Image,

        [JsonStringEnumMemberName("video")]
        Video,

        [JsonStringEnumMemberName("gifv")]
        Gifv,

        [JsonStringEnumMemberName("article")]
        Article,

        [JsonStringEnumMemberName("link")]
        Link,

        [JsonStringEnumMemberName("poll_result")]
        Poll_result
    }
}