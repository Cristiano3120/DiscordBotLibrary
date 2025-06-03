using System.Text.Json;

namespace DiscordBotLibrary.Json
{
    public sealed class SnowflakeArrayConverter : JsonConverter<ulong[]>
    {
        public override ulong[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new Exception("Expected start of array token.");

            List<ulong> snowflakes = [];
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    return [.. snowflakes];

                string? snowflakeString = reader.GetString();
                if (ulong.TryParse(snowflakeString, out ulong snowflake))
                {
                    snowflakes.Add(snowflake);
                }
                else
                {
                    throw new JsonException($"Invalid Snowflake format: {snowflakeString}");
                }
            }

            return [.. snowflakes];
        }

        public override void Write(Utf8JsonWriter writer, ulong[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (ulong snowflake in value)
            {
                writer.WriteStringValue(snowflake.ToString());
            }
            writer.WriteEndArray();
        }
    }
}
