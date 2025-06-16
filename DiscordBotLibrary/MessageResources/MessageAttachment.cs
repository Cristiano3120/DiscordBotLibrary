namespace DiscordBotLibrary.MessageResources
{
    public sealed record MessageAttachment
    {
        /// <summary>
        /// attachment id
        /// </summary>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(SnowflakeConverter))]
        public ulong Id { get; init; }

        /// <summary>
        /// name of file attached
        /// </summary>
        [JsonPropertyName("filename")]
        public string Filename { get; init; } = default!;

        /// <summary>
        /// the title of the file
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; init; }

        /// <summary>
        /// description for the file (max 1024 characters)
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; init; }

        /// <summary>
        /// the attachment's media type
        /// </summary>
        [JsonPropertyName("content_type")]
        public string? ContentType { get; init; }

        /// <summary>
        /// size of file in bytes
        /// </summary>
        [JsonPropertyName("size")]
        public int Size { get; init; }

        /// <summary>
        /// source url of file
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; init; } = default!;

        /// <summary>
        /// a proxied url of file
        /// </summary>
        [JsonPropertyName("proxy_url")]
        public string ProxyUrl { get; init; } = default!;

        /// <summary>
        /// height of file (if image)
        /// </summary>
        [JsonPropertyName("height")]
        public int? Height { get; init; }

        /// <summary>
        /// width of file (if image)
        /// </summary>
        [JsonPropertyName("width")]
        public int? Width { get; init; }

        /// <summary>
        /// whether this attachment is ephemeral
        /// </summary>
        [JsonPropertyName("ephemeral")]
        public bool? Ephemeral { get; init; }

        /// <summary>
        /// the duration of the audio file (currently for voice messages)
        /// </summary>
        [JsonPropertyName("duration_secs")]
        public float? DurationSecs { get; init; }

        /// <summary>
        /// base64 encoded bytearray representing a sampled waveform (currently for voice messages)
        /// </summary>
        [JsonPropertyName("waveform")]
        public string? Waveform { get; init; }

        /// <summary>
        /// attachment flags combined as a bitfield
        /// </summary>
        [JsonPropertyName("flags")]
        public MessageAttachmentFlags? Flags { get; init; }
    }
}
