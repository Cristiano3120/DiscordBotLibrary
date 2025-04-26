using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    internal readonly struct WelcomeScreen
    {
        public string? Description { get; init; }

        [JsonPropertyName("welcome_channels")]
        public List<WelcomeScreenChannel> WelcomeChannels { get; init; }
    }
}
