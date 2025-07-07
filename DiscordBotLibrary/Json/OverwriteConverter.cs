namespace DiscordBotLibrary.Json
{
    internal sealed class OverwriteConverter : JsonConverter<Overwrite>
    {
        public override Overwrite Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            JsonElement root = document.RootElement;

            if (!ulong.TryParse(root.GetProperty("allow").GetString(), out ulong allowBits))
            {
                allowBits = 0;
            }

            if (!ulong.TryParse(root.GetProperty("deny").GetString(), out ulong denyBits))
            {
                denyBits = 0;
            }

            return new Overwrite
            {
                Id = ulong.Parse(root.GetProperty("id").GetString()!),
                Type = (OverwriteType)root.GetProperty("type").GetInt32(),
                Allow = (DiscordPermissions)allowBits,
                Deny = (DiscordPermissions)denyBits
            };
        }

        public override void Write(Utf8JsonWriter writer, Overwrite value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id.ToString());
            writer.WriteNumber("type", (byte)value.Type);
            writer.WriteString("allow", ((ulong)value.Allow).ToString());
            writer.WriteString("deny", ((ulong)value.Deny).ToString());
            writer.WriteEndObject();
        }
    }
}
