using System.Runtime.Serialization;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents Discord-supported user locales.
    /// </summary>
    public enum Language
    {
        [JsonStringEnumMemberName("id")]
        Indonesian,

        [JsonStringEnumMemberName("da")]
        Danish,

        [JsonStringEnumMemberName("de")]
        German,

        [JsonStringEnumMemberName("en-GB")]
        EnglishUK,

        [JsonStringEnumMemberName("en-US")]
        EnglishUS,

        [JsonStringEnumMemberName("es-ES")]
        Spanish,

        [JsonStringEnumMemberName("es-419")]
        SpanishLATAM,

        [JsonStringEnumMemberName("fr")]
        French,

        [JsonStringEnumMemberName("hr")]
        Croatian,

        [JsonStringEnumMemberName("it")]
        Italian,

        [JsonStringEnumMemberName("lt")]
        Lithuanian,

        [JsonStringEnumMemberName("hu")]
        Hungarian,

        [JsonStringEnumMemberName("nl")]
        Dutch,

        [JsonStringEnumMemberName("no")]
        Norwegian,

        [JsonStringEnumMemberName("pl")]
        Polish,

        [JsonStringEnumMemberName("pt-BR")]
        PortugueseBrazilian,

        [JsonStringEnumMemberName("ro")]
        Romanian,

        [JsonStringEnumMemberName("fi")]
        Finnish,

        [JsonStringEnumMemberName("sv-SE")]
        Swedish,

        [JsonStringEnumMemberName("vi")]
        Vietnamese,

        [JsonStringEnumMemberName("tr")]
        Turkish,

        [JsonStringEnumMemberName("cs")]
        Czech,

        [JsonStringEnumMemberName("el")]
        Greek,

        [JsonStringEnumMemberName("bg")]
        Bulgarian,

        [JsonStringEnumMemberName("ru")]
        Russian,

        [JsonStringEnumMemberName("uk")]
        Ukrainian,

        [JsonStringEnumMemberName("hi")]
        Hindi,

        [JsonStringEnumMemberName("th")]
        Thai,

        [JsonStringEnumMemberName("zh-CN")]
        ChineseChina,

        [JsonStringEnumMemberName("ja")]
        Japanese,

        [JsonStringEnumMemberName("zh-TW")]
        ChineseTaiwan,

        [JsonStringEnumMemberName("ko")]
        Korean
    }
}
