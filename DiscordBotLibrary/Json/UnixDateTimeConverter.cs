namespace DiscordBotLibrary.Json
{
    internal class UnixDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.TryGetInt64(out long unixMilliseconds)
                ? DateTimeOffset.FromUnixTimeMilliseconds(unixMilliseconds).UtcDateTime
                : null;
        

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            long unixMilliseconds = new DateTimeOffset(value.Value).ToUnixTimeMilliseconds();
            writer.WriteNumberValue(unixMilliseconds);
        }
    }
}
