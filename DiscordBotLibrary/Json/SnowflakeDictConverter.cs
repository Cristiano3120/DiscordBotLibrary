namespace DiscordBotLibrary.Json
{
    internal class SnowflakeDictConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>> where TKey : notnull
    {
        public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<TKey, TValue> dict = new();
            bool valueIsUlong = typeof(TValue) == typeof(ulong);
            bool keyIsUlong = typeof(TKey) == typeof(ulong);

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return dict;

                string keyString = reader.GetString()!;
                reader.Read();

                object key = keyIsUlong 
                    ? ulong.Parse(keyString) 
                    : JsonSerializer.Deserialize<TKey>(JsonDocument.Parse($"\"{keyString}\"").RootElement.GetRawText(), options)!;
                
                object value = valueIsUlong 
                    ? reader.GetUInt64() 
                    : JsonSerializer.Deserialize<TValue>(ref reader, options)!;

                dict.Add((TKey)key, (TValue)value);
            }

            throw new JsonException("Invalid JSON for dictionary.");
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
        {
            bool keyIsUlong = typeof(TKey) == typeof(ulong);
            bool valueIsUlong = typeof(TValue) == typeof(ulong);

            writer.WriteStartObject();

            foreach (KeyValuePair<TKey, TValue> kv in value)
            {
                string keyStr = keyIsUlong ? kv.Key!.ToString()! : JsonSerializer.Serialize(kv.Key, options).Trim('"');
                writer.WritePropertyName(keyStr);

                if (valueIsUlong)
                    writer.WriteNumberValue(Convert.ToUInt64(kv.Value));
                else
                    JsonSerializer.Serialize(writer, kv.Value, options);
            }

            writer.WriteEndObject();
        }
    }
}
