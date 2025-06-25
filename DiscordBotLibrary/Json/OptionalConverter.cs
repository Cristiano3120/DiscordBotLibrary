namespace DiscordBotLibrary.Json
{
    internal class OptionalConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type innerType = typeToConvert.GetGenericArguments()[0];
            Type converterType = typeof(OptionalInnerConverter<>).MakeGenericType(innerType);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }

        private class OptionalInnerConverter<T> : JsonConverter<Optional<T>>
        {
            public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                T value = JsonSerializer.Deserialize<T>(ref reader, options)!;
                return new Optional<T>(value);
            }

            public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                    JsonSerializer.Serialize(writer, value.Value, options);
            }
        }
    }
}
