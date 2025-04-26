using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    internal readonly struct IncidentsData
    {
        [JsonPropertyName("is_critical")]
        public bool IsCritical { get; init; }

        [JsonPropertyName("incident_id")]
        public string? IncidentId { get; init; }
        public string? Maintenance { get; init; }
    }
}