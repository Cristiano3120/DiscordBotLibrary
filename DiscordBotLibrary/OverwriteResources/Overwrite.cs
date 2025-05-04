using System.Text.Json.Serialization;

namespace DiscordBotLibrary.OverwriteResources
{
    public readonly struct Overwrite
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("type")]
        public OverwriteType Type { get; init; }

        [JsonPropertyName("allow")]
        public string Allow { get; init; }

        [JsonPropertyName("deny")]
        public string Deny { get; init; }
    }
}
