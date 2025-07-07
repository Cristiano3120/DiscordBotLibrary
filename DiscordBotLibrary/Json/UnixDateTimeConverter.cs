namespace DiscordBotLibrary.Json
{
    internal class UnixDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.Integer)
            {
                long unixMilliseconds = Convert.ToInt64(reader.Value);
                return DateTimeOffset.FromUnixTimeMilliseconds(unixMilliseconds).UtcDateTime;
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            long unixMilliseconds = new DateTimeOffset(value.Value).ToUnixTimeMilliseconds();
            writer.WriteValue(unixMilliseconds);
        }
    }
}
