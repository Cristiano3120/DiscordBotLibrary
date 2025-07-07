using System.Runtime.Serialization;

namespace DiscordBotLibrary.PresenceUpdateResources
{
    /// <summary>
    /// Represents the user's current presence status.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PresenceStatus : byte
    {
        [EnumMember(Value = "online")]
        Online,

        [EnumMember(Value = "idle")]
        Idle,

        [EnumMember(Value = "dnd")]
        DoNotDisturb,

        [EnumMember(Value = "offline")]
        Offline
    }
}