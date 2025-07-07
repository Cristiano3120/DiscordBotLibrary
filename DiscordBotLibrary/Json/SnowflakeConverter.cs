namespace DiscordBotLibrary.Json
{
    public class SnowflakeConverter : JsonConverter<ulong>
    {
        public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? snowflakeString = reader.GetString();

            if (ulong.TryParse(snowflakeString, out ulong result))
            {
                return result;
            }

            throw new JsonException("Invalid Snowflake format");
        }

        public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
