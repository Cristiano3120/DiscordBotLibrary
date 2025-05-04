using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DiscordBotLibrary.PresenceUpdateResources
{
    /// <summary>
    /// Represents the user's current presence status.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PresenceStatus
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