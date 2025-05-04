using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    public readonly struct IncidentsData
    {
        [JsonPropertyName("is_critical")]
        public bool IsCritical { get; init; }

        [JsonPropertyName("incident_id")]
        public string? IncidentId { get; init; }
        public string? Maintenance { get; init; }
    }
}