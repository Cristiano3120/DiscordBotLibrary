namespace DiscordBotLibrary.Json
{
    internal static class JTokenExtensions
    {
        /// <summary>
        /// Gets the data["d"] property from the JToken and deserializes it to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T Deserialize<T>(this JToken token)
            => JsonConvert.DeserializeObject<T>(token["d"]!.ToString(), DiscordClient.ReceiveJsonSerializerOptions) 
                ?? throw new JsonException("d property is missing");
        
        public static JToken GetProperty(this JToken token, string propertyName)
        {
            ArgumentNullException.ThrowIfNull(token);
            if (token.Type != JTokenType.Object)
                throw new InvalidOperationException("GetProperty can only be called on JSON objects.");

            JToken? prop = token[propertyName];
            return prop is null 
                ? throw new Exception($"Property '{propertyName}' not found.") 
                : prop;
        }

        public static bool TryGetProperty(this JToken token, string propertyName, out JToken? value)
        {
            value = null;
            if (token == null || token.Type != JTokenType.Object)
                return false;

            JObject obj = (JObject)token;
            return obj.TryGetValue(propertyName, out value);
        }

        internal static OpCode GetOpCode(this JToken token)
        {
            JToken? opCodeToken = token["op"];
            return opCodeToken != null && byte.TryParse(opCodeToken.ToString(), out byte opCode)
                ? (OpCode)opCode
                : throw new JsonException("Payload data invalid!");
        }

        internal static int GetSequenceNumber(this JToken token)
        {
            JToken? seqToken = token["s"];
            return seqToken != null && int.TryParse(seqToken.ToString(), out int sequenceNumber)
                ? sequenceNumber
                : throw new JsonException("Payload data invalid!");
        }

        internal static Event GetEvent(this JToken token)
        {
            JToken? eventToken = token["t"];
            if (eventToken != null)
            {
                string? eventName = eventToken.ToString();
                if (!string.IsNullOrEmpty(eventName))
                    return Enum.Parse<Event>(eventName);
            }

            throw new JsonException("Invalid Payload (t)");
        }
    }
}
