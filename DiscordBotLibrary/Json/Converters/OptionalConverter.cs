using Newtonsoft.Json.Linq;
using System.Reflection;

namespace DiscordBotLibrary.Json.Converters
{
    internal class OptionalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Optional<>);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            Type valueType = objectType.GetGenericArguments()[0];
            JToken token = JToken.Load(reader);
            object? value = token.ToObject(valueType, serializer);

            Type optionalType = typeof(Optional<>).MakeGenericType(valueType);
            return Activator.CreateInstance(optionalType, value);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
                return;

            Type valueType = value.GetType();
            PropertyInfo hasValueProp = valueType.GetProperty("HasValue")!;
            bool hasValue = (bool)hasValueProp.GetValue(value)!;

            if (hasValue)
            {
                PropertyInfo valueProp = valueType.GetProperty("Value")!;
                object? innerValue = valueProp.GetValue(value);
                serializer.Serialize(writer, innerValue);
            }
        }
    }
}
