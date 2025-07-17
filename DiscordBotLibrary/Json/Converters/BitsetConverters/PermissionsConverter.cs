namespace DiscordBotLibrary.Json.Converters.BitsetConverters
{
    internal class PermissionsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(DiscordPermissions) || objectType == typeof(DiscordPermissions?);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return objectType == typeof(DiscordPermissions)
                    ? throw new JsonSerializationException($"Snowflake at {reader.Path} must not be null.")
                    : null;
            }

            string bitsetStr = (string)reader.Value!;
            return bitsetStr is not null && ulong.TryParse(bitsetStr, out ulong bitset)
                ? (DiscordPermissions)bitset
                : throw new ArgumentNullException(nameof(bitset));
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
                return;
            }

            ulong valueAsNum = (ulong)value;
            writer.WriteValue(valueAsNum.ToString());
        }
    }
}
