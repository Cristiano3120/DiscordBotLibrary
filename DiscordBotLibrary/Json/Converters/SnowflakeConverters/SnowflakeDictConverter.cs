namespace DiscordBotLibrary.Json.Converters.SnowflakeConverters
{
    internal class SnowflakeDictConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>> where TKey : notnull
    {
        public override Dictionary<TKey, TValue>? ReadJson(JsonReader reader, Type objectType, Dictionary<TKey, TValue>? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
                throw new JsonSerializationException("Expected StartObject");

            Dictionary<TKey, TValue> dict = new();
            JObject obj = JObject.Load(reader);

            bool keyIsUlong = typeof(TKey) == typeof(ulong);
            bool valueIsUlong = typeof(TValue) == typeof(ulong);

            foreach (JProperty property in obj.Properties())
            {
                TKey key = keyIsUlong
                    ? (TKey)(object)ulong.Parse(property.Name)
                    : serializer.Deserialize<TKey>(new JTokenReader(JValue.CreateString(property.Name)))!;
                TValue value = valueIsUlong ? (TValue)(object)property.Value.Value<ulong>() : property.Value.ToObject<TValue>(serializer)!;
                dict.Add(key, value);
            }

            return dict;
        }

        public override void WriteJson(JsonWriter writer, Dictionary<TKey, TValue>? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            bool keyIsUlong = typeof(TKey) == typeof(ulong);
            bool valueIsUlong = typeof(TValue) == typeof(ulong);

            foreach (KeyValuePair<TKey, TValue> kvp in value)
            {
                string keyStr = keyIsUlong
                    ? kvp.Key!.ToString()!
                    : JToken.FromObject(kvp.Key, serializer).ToString(Formatting.None).Trim('"');

                writer.WritePropertyName(keyStr);

                if (valueIsUlong)
                {
                    writer.WriteValue(Convert.ToUInt64(kvp.Value));
                }
                else
                {
                    serializer.Serialize(writer, kvp.Value);
                }
            }

            writer.WriteEndObject();
        }
    }
}
