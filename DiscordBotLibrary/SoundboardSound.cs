using System.Text.Json.Serialization;

namespace DiscordBotLibrary
{
    /// <summary>
    /// Represents a soundboard sound in a Discord server.
    /// </summary>
    public sealed record SoundboardSound
    {
        /// <summary>
        /// The name of this sound
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// The id of this sound
        /// TYPE: SNOWFLAKE
        /// </summary>
        [JsonPropertyName("sound_id")]
        public string SoundId { get; init; } = string.Empty;

        /// <summary>
        /// The volume of this sound.
        /// Range from 0.0 to 1.0.
        /// </summary>
        [JsonPropertyName("volume")]
        public double Volume { get; init; }

        /// <summary>
        /// The id of this sound's custom emoji
        /// TYPE: SNOWFLAKE
        /// </summary>
        [JsonPropertyName("emoji_name")]
        public string? EmojiId { get; init; }

        /// <summary>
        /// The unicode character of this sound's standard emoji
        /// </summary>
        [JsonPropertyName("emoji_name")]
        public string? EmojiName { get; init; }

        /// <summary>
        /// The id of the guild this sound is in
        /// TYPE: SNOWFLAKE
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string? GuildId { get; init; } = string.Empty;

        /// <summary>
        /// Whether this sound can be used, may be false due to loss of Server Boosts
        /// </summary>
        [JsonPropertyName("available")]
        public bool Available { get; init; }

        /// <summary>
        /// The id of the user who created this sound
        /// </summary>
        [JsonPropertyName("user")]
        public User? User { get; init; }
    }
}