namespace DiscordBotLibrary
{
    [JsonConverter(typeof(OptionalConverter))]
    public readonly struct Optional<T>
    {
        public bool HasValue { get; }

        public T? Value { get; }

        internal Optional(T? value)
        {
            Value = value;
            HasValue = true;
        }

        public static implicit operator Optional<T>(T? value) => new(value);
    }
}
