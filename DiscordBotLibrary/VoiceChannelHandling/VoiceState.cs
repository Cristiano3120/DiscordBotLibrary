using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.VoiceChannelHandling
{
    /// <summary>
    /// Represents a voice state in a Discord server.
    /// </summary>
    public sealed record VoiceState
    {
        /// <summary>
        /// the guild id this voice state is for
        /// </summary>
        [JsonProperty("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; init; }

        /// <summary>
        /// the channel id this user is connected to
        /// </summary>
        [JsonProperty("channel_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? ChannelId { get; init; }

        /// <summary>
        /// the user id this voice state is for
        /// </summary>
        [JsonProperty("user_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong UserId { get; init; }

        /// <summary>
        /// the guild member this voice state is for
        /// </summary>
        [JsonProperty("member")]
        public GuildMember? Member { get; init; }

        /// <summary>
        /// the session id for this voice state
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; init; } = default!;

        /// <summary>
        /// whether this user is deafened by the server
        /// </summary>
        [JsonProperty("deaf")]
        public bool Deaf { get; init; }

        /// <summary>
        /// whether this user is muted by the server
        /// </summary>
        [JsonProperty("mute")]
        public bool Mute { get; init; }

        /// <summary>
        /// whether this user is locally deafened
        /// </summary>
        [JsonProperty("self_deaf")]
        public bool SelfDeaf { get; init; }

        /// <summary>
        /// whether this user is locally muted
        /// </summary>
        [JsonProperty("self_mute")]
        public bool SelfMute { get; init; }

        /// <summary>
        /// whether this user is streaming using "Go Live"
        /// </summary>
        [JsonProperty("self_stream")]
        public bool? SelfStream { get; init; }

        /// <summary>
        /// whether this user's camera is enabled
        /// </summary>
        [JsonProperty("self_video")]
        public bool SelfVideo { get; init; }

        /// <summary>
        /// whether this user's permission to speak is denied
        /// </summary>
        [JsonProperty("suppress")]
        public bool Suppress { get; init; }

        [JsonProperty("discoverable")]
        public bool Discoverable { get; init; }

        /// <summary>
        /// the time at which the user requested to speak
        /// </summary>
        [JsonProperty("request_to_speak_timestamp")]
        public DateTimeOffset? RequestToSpeakTimestamp { get; init; }
    }
}
