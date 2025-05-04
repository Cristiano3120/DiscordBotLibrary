using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    public readonly struct ThreadMetadata
    {
        [JsonPropertyName("archived")]
        public bool Archived { get; init; }

        [JsonPropertyName("auto_archive_duration")]
        public int AutoArchiveDuration { get; init; }

        [JsonPropertyName("archive_timestamp")]
        public DateTime ArchiveTimestamp { get; init; }

        [JsonPropertyName("locked")]
        public bool Locked { get; init; }

        [JsonPropertyName("invitable")]
        public bool? Invitable { get; init; }

        [JsonPropertyName("create_timestamp")]
        public DateTime? CreateTimestamp { get; init; }
    }
}
