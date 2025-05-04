using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    internal record ReadyEventArgs
    {
        [JsonPropertyName("v")]
        public int Version { get; init; }

        [JsonPropertyName("user")]
        public User? DiscordUser { get; init; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; init; } = "";

        [JsonPropertyName("resume_gateway_url")]
        public string ResumeGatewayUri { get; init; } = "";

        public List<UnavailableGuild> Guilds { get; init; } = [];

        [JsonPropertyName("geo_ordered_rtc_regions")]
        public List<string> GeoOrderedRtcRegions { get; init; } = [];
        public List<object> Relationships { get; init; } = [];

        [JsonPropertyName("private_channels")]
        public List<object> PrivateChannels { get; init; } = [];

        [JsonPropertyName("user_settings")]
        public object? UserSettings { get; init; }
    }
}
