namespace DiscordBotLibrary.Json.Converters.SnowflakeConverters
{
    public class SnowflakeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(ulong) || objectType == typeof(ulong?);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return objectType == typeof(ulong) 
                    ? throw new JsonSerializationException($"Snowflake at {reader.Path} must not be null.") 
                    : null;
            }

            string? snowflakeString = reader.Value?.ToString();

            return ulong.TryParse(snowflakeString, out ulong result)
                ? (object)result
                : throw new JsonSerializationException($"Invalid Snowflake format at {reader.Path}");
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is ulong snowflake)
                writer.WriteValue(snowflake.ToString());
            else
                writer.WriteNull();
        }
    }
}
