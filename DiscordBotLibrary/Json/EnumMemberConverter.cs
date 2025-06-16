using System.Reflection;
using System.Runtime.Serialization;

namespace DiscordBotLibrary.Json
{
    public class EnumMemberConverter<T> : JsonConverter<T> where T : Enum
    {
        private readonly Dictionary<string, T> _fromValue = [];
        private readonly Dictionary<T, string> _toValue = [];

        public EnumMemberConverter()
        {
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                T enumValue = (T)field.GetValue(null)!;
                string name = field.Name;

                EnumMemberAttribute? attr = field.GetCustomAttribute<EnumMemberAttribute>();
                string value = attr?.Value ?? name;

                _fromValue[value] = enumValue;
                _toValue[enumValue] = value;
            }
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? key = reader.GetString();
            if (key != null && _fromValue.TryGetValue(key, out var enumValue))
                return enumValue;

            throw new JsonException($"Unable to convert \"{key}\" to Enum \"{typeof(T)}\".");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            string stringValue = _toValue[value];
            writer.WriteStringValue(stringValue);
        }
    }
}
