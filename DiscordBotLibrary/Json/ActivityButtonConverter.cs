namespace DiscordBotLibrary.Json
{
    public class ActivityButtonConverter : JsonConverter<ActivityButton[]>
    {
        public override ActivityButton[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<ActivityButton> result = [];

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                if (reader.TokenType == JsonTokenType.String)
                {
                    string? label = reader.GetString();
                    result.Add(new ActivityButton { Label = label, Url = null });
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    using JsonDocument doc = JsonDocument.ParseValue(ref reader);
                    {
                        JsonElement root = doc.RootElement;

                        string? label = root.TryGetProperty("label", out JsonElement l)
                            ? l.GetString()
                            : null;

                        string? url = root.TryGetProperty("url", out JsonElement u)
                            ? u.GetString()
                            : null;

                        result.Add(new ActivityButton { Label = label, Url = url });
                    }  
                }
            }

            return [.. result];
        }

        public override void Write(Utf8JsonWriter writer, ActivityButton[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (ActivityButton button in value)
            {
                if (button.Url is null)
                {
                    writer.WriteStringValue(button.Label);
                }
                else
                {
                    writer.WriteStartObject();
                    writer.WriteString("label", button.Label);
                    writer.WriteString("url", button.Url);
                    writer.WriteEndObject();
                }
            }

            writer.WriteEndArray();
        }
    }
}
