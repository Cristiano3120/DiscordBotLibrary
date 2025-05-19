using System.Diagnostics;

namespace DiscordBotLibrary.ExternalExtraClasses
{
    /// <summary>
    /// Represents the presence update payload sent by the client to Discord to set its status.
    /// </summary>
    public sealed record SelfPresenceUpdate
    {
        /// <summary>
        /// Timestamp of when the client went idle, or null if not idle.
        /// </summary>
        [JsonPropertyName("since")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? Since { get; init; }

        /// <summary>
        /// The activities the user is currently engaged in.
        /// Bots can only set the activity propertys: <c> name, state, type, and url</c>/>
        /// </summary>
        [JsonPropertyName("activities")]
        public ActivityResources.Activity[] Activities { get; init; } = [];

        /// <summary>
        /// The user's new status (e.g. "online", "idle", "dnd", "invisible").
        /// </summary>
        [JsonPropertyName("status")]
        public PresenceStatus Status { get; init; }

        /// <summary>
        /// Whether the user is currently away from keyboard.
        /// </summary>
        [JsonPropertyName("afk")]
        public bool Afk { get; init; } = false;
    }
}
