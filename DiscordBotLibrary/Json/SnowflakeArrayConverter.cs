namespace DiscordBotLibrary.Json
{
    public sealed class SnowflakeArrayConverter : JsonConverter<ulong[]>
    {
        public override ulong[]? ReadJson(JsonReader reader, Type objectType, ulong[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonSerializationException("Expected start of array.");

            List<ulong> snowflakes = new();
            JArray array = JArray.Load(reader);

            foreach (JToken token in array)
            {
                string? snowflakeString = token?.ToString();
                if (ulong.TryParse(snowflakeString, out ulong snowflake))
                {
                    snowflakes.Add(snowflake);
                }
                else
                {
                    throw new JsonSerializationException($"Invalid Snowflake format: {snowflakeString}");
                }
            }

            return [.. snowflakes];
        }

        public override void WriteJson(JsonWriter writer, ulong[]? value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            if (value != null)
            {
                foreach (ulong snowflake in value)
                {
                    writer.WriteValue(snowflake.ToString());
                }
            }
            writer.WriteEndArray();
        }
    }

}
