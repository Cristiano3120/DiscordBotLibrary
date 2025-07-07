namespace DiscordBotLibrary.Json
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;

    internal sealed class OverwriteConverter : JsonConverter<Overwrite>
    {
        public override Overwrite ReadJson(JsonReader reader, Type objectType, Overwrite existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            ulong allowBits = ulong.TryParse((string?)obj["allow"], out ulong a) ? a : 0;
            ulong denyBits = ulong.TryParse((string?)obj["deny"], out ulong d) ? d : 0;

            return new Overwrite
            {
                Id = ulong.Parse((string?)obj["id"] ?? "0"),
                Type = (OverwriteType)((int?)obj["type"] ?? 0),
                Allow = (DiscordPermissions)allowBits,
                Deny = (DiscordPermissions)denyBits
            };
        }

        public override void WriteJson(JsonWriter writer, Overwrite value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("id");
            writer.WriteValue(value.Id.ToString());

            writer.WritePropertyName("type");
            writer.WriteValue((byte)value.Type);

            writer.WritePropertyName("allow");
            writer.WriteValue(((ulong)value.Allow).ToString());

            writer.WritePropertyName("deny");
            writer.WriteValue(((ulong)value.Deny).ToString());
            writer.WriteEndObject();
        }
    }

}
