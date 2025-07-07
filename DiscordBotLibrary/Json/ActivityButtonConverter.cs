using Newtonsoft.Json.Linq;

namespace DiscordBotLibrary.Json
{
    public class ActivityButtonConverter : JsonConverter<ActivityButton[]>
    {
        public override ActivityButton[]? ReadJson(JsonReader reader, Type objectType, ActivityButton[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartArray)
                throw new JsonSerializationException("Expected start of array for ActivityButton[].");

            var result = new List<ActivityButton>();

            JArray array = JArray.Load(reader);

            foreach (JToken item in array)
            {
                switch (item.Type)
                {
                    case JTokenType.String:
                        result.Add(new ActivityButton
                        {
                            Label = item.ToString(),
                            Url = null
                        });
                        break;

                    case JTokenType.Object:
                        var label = item["label"]?.ToString();
                        var url = item["url"]?.ToString();

                        if (label == null)
                            throw new JsonSerializationException("Missing 'label' in ActivityButton object.");

                        result.Add(new ActivityButton
                        {
                            Label = label,
                            Url = url
                        });
                        break;

                    default:
                        throw new JsonSerializationException($"Unexpected token type {item.Type} in ActivityButton array.");
                }
            }

            return result.ToArray();
        }

        public override void WriteJson(JsonWriter writer, ActivityButton[]? value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            foreach (ActivityButton button in value ?? [])
            {
                if (button.Url == null)
                {
                    writer.WriteValue(button.Label);
                }
                else
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("label");
                    writer.WriteValue(button.Label);
                    writer.WritePropertyName("url");
                    writer.WriteValue(button.Url);
                    writer.WriteEndObject();
                }
            }

            writer.WriteEndArray();
        }
    }
}
