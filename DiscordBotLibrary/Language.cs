using System.Runtime.Serialization;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents Discord-supported user locales.
    /// </summary>
    public enum Language
    {
        [EnumMember(Value = "id")]
        Indonesian,

        [EnumMember(Value = "da")]
        Danish,

        [EnumMember(Value = "de")]
        German,

        [EnumMember(Value = "en-GB")]
        EnglishUK,

        [EnumMember(Value = "en-US")]
        EnglishUS,

        [EnumMember(Value = "es-ES")]
        Spanish,

        [EnumMember(Value = "es-419")]
        SpanishLATAM,

        [EnumMember(Value = "fr")]
        French,

        [EnumMember(Value = "hr")]
        Croatian,

        [EnumMember(Value = "it")]
        Italian,

        [EnumMember(Value = "lt")]
        Lithuanian,

        [EnumMember(Value = "hu")]
        Hungarian,

        [EnumMember(Value = "nl")]
        Dutch,

        [EnumMember(Value = "no")]
        Norwegian,

        [EnumMember(Value = "pl")]
        Polish,

        [EnumMember(Value = "pt-BR")]
        PortugueseBrazilian,

        [EnumMember(Value = "ro")]
        Romanian,

        [EnumMember(Value = "fi")]
        Finnish,

        [EnumMember(Value = "sv-SE")]
        Swedish,

        [EnumMember(Value = "vi")]
        Vietnamese,

        [EnumMember(Value = "tr")]
        Turkish,

        [EnumMember(Value = "cs")]
        Czech,

        [EnumMember(Value = "el")]
        Greek,

        [EnumMember(Value = "bg")]
        Bulgarian,

        [EnumMember(Value = "ru")]
        Russian,

        [EnumMember(Value = "uk")]
        Ukrainian,

        [EnumMember(Value = "hi")]
        Hindi,

        [EnumMember(Value = "th")]
        Thai,

        [EnumMember(Value = "zh-CN")]
        ChineseChina,

        [EnumMember(Value = "ja")]
        Japanese,

        [EnumMember(Value = "zh-TW")]
        ChineseTaiwan,

        [EnumMember(Value = "ko")]
        Korean
    }
}
