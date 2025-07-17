using DiscordBotLibrary.Json.Converters.SnowflakeConverters;

namespace DiscordBotLibrary.MessageResources
{
    public sealed record MessageAttachment
    {
        /// <summary>
        /// attachment id
        /// </summary>
        [JsonProperty("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; }

        /// <summary>
        /// name of file attached
        /// </summary>
        [JsonProperty("filename")]
        public string Filename { get; init; } = default!;

        /// <summary>
        /// the title of the file
        /// </summary>
        [JsonProperty("title")]
        public string? Title { get; init; }

        /// <summary>
        /// description for the file (max 1024 characters)
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; init; }

        /// <summary>
        /// the attachment's media type
        /// </summary>
        [JsonProperty("content_type")]
        public string? ContentType { get; init; }

        /// <summary>
        /// size of file in bytes
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; init; }

        /// <summary>
        /// source url of file
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; init; } = default!;

        /// <summary>
        /// a proxied url of file
        /// </summary>
        [JsonProperty("proxy_url")]
        public string ProxyUrl { get; init; } = default!;

        /// <summary>
        /// height of file (if image)
        /// </summary>
        [JsonProperty("height")]
        public int? Height { get; init; }

        /// <summary>
        /// width of file (if image)
        /// </summary>
        [JsonProperty("width")]
        public int? Width { get; init; }

        /// <summary>
        /// whether this attachment is ephemeral
        /// </summary>
        [JsonProperty("ephemeral")]
        public bool? Ephemeral { get; init; }

        /// <summary>
        /// the duration of the audio file (currently for voice messages)
        /// </summary>
        [JsonProperty("duration_secs")]
        public float? DurationSecs { get; init; }

        /// <summary>
        /// base64 encoded bytearray representing a sampled waveform (currently for voice messages)
        /// </summary>
        [JsonProperty("waveform")]
        public string? Waveform { get; init; }

        /// <summary>
        /// attachment flags combined as a bitfield
        /// </summary>
        [JsonProperty("flags")]
        public MessageAttachmentFlags? Flags { get; init; }
    }
}
