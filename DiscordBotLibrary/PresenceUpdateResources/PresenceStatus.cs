using System.Runtime.Serialization;

namespace DiscordBotLibrary.PresenceUpdateResources
{
    /// <summary>
    /// Represents the user's current presence status.
    /// </summary>
    public enum PresenceStatus : byte
    {
        [JsonStringEnumMemberName("online")]
        Online,

        [JsonStringEnumMemberName("idle")]
        Idle,

        [JsonStringEnumMemberName("dnd")]
        DoNotDisturb,

        [JsonStringEnumMemberName("offline")]
        Offline
    }
}