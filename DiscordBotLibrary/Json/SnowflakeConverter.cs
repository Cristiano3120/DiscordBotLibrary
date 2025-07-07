namespace DiscordBotLibrary.Json
{
    public class SnowflakeConverter : JsonConverter<ulong>
    {
        public override ulong ReadJson(JsonReader reader, Type objectType, ulong existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? snowflakeString = reader.Value?.ToString();

            return ulong.TryParse(snowflakeString, out ulong result) 
                ? result 
                : throw new JsonSerializationException("Invalid Snowflake format");
        }

        public override void WriteJson(JsonWriter writer, ulong value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
