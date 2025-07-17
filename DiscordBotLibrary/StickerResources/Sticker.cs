using DiscordBotLibrary.Json.Converters.SnowflakeConverters;
using DiscordBotLibrary.UserResources;

namespace DiscordBotLibrary.StickerResources
{
    /// <summary>
    /// Represents a sticker object used within Discord.
    /// Stickers can be standard (from a pack) or custom (uploaded to a guild).
    /// </summary>
    public sealed record Sticker
    {
        /// <summary>
        /// The ID of the sticker.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; }

        /// <summary>
        /// For standard stickers, the ID of the pack this sticker belongs to.
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("pack_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? PackId { get; init; }

        /// <summary>
        /// The name of the sticker.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// The description of the sticker (if any).
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; init; }

        /// <summary>
        /// Autocomplete/suggestion tags for the sticker (max 200 characters).
        /// Comma-separated string.
        /// </summary>
        [JsonProperty("tags")]
        public string Tags { get; init; } = string.Empty;

        /// <summary>
        /// The type of the sticker.
        /// See <see href="https://discord.com/developers/docs/resources/sticker#sticker-object-sticker-types">Sticker Types</see>.
        /// </summary>
        [JsonProperty("type")]
        public StickerType Type { get; init; } = default!;

        /// <summary>
        /// The format type of the sticker.
        /// See <see href="https://discord.com/developers/docs/resources/sticker#sticker-object-sticker-format-types">Sticker Format Types</see>.
        /// </summary>
        [JsonProperty("format_type")]
        public StickerFormatType FormatType { get; init; } = default!;

        /// <summary>
        /// Whether this guild sticker can currently be used.
        /// Might be false if the server lost boost level.
        /// </summary>
        [JsonProperty("available")]
        public bool Available { get; init; }

        /// <summary>
        /// The ID of the guild this sticker belongs to (if any).
        /// TYPE: Snowflake
        /// </summary>
        [JsonProperty("guild_id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong? GuildId { get; init; }

        /// <summary>
        /// The user who uploaded the guild sticker (only for guild stickers).
        /// </summary>
        [JsonProperty("user")]
        public User? User { get; init; }

        /// <summary>
        /// The sort order of the standard sticker within its pack (if applicable).
        /// </summary>
        [JsonProperty("sort_value")]
        public int? SortValue { get; init; }
    }

}