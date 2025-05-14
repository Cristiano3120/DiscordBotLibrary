using DiscordBotLibrary.UserResources;

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
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("sound_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong SoundId { get; init; }

        /// <summary>
        /// The volume of this sound.
        /// Range from 0.0 to 1.0.
        /// </summary>
        [JsonPropertyName("volume")]
        public double Volume { get; init; }

        /// <summary>
        /// The id of this sound's custom emoji
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("emoji_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? EmojiId { get; init; }

        /// <summary>
        /// The unicode character of this sound's standard emoji
        /// </summary>
        [JsonPropertyName("emoji_name")]
        public string? EmojiName { get; init; }

        /// <summary>
        /// The id of the guild this sound is in
        /// TYPE: Snowflake
        /// </summary>
        [JsonPropertyName("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; init; }

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