using System.Text.Json;

namespace DiscordBotLibrary
{
    internal static class JsonExtensions
    {
        internal static OpCode GetOpCode(this JsonElement jsonElement)
        {
            if (jsonElement.TryGetProperty("op", out JsonElement opCodeElement))
            {
                if (opCodeElement.TryGetByte(out byte opCode))
                {
                    return (OpCode)opCode;
                }
            }

            throw new JsonException("Invalid Payload");
        }

        internal static int GetSequenceNumber(this JsonElement jsonElement)
        {
            if (jsonElement.TryGetProperty("s", out JsonElement sequenceNumberElement))
            {
                if (sequenceNumberElement.TryGetInt32(out int sequenceNumber))
                {
                    return sequenceNumber;
                }
            }

            throw new JsonException("Invalid Payload");
        }

        internal static Events GetEvent(this JsonElement jsonElement)
        {
            if (jsonElement.TryGetProperty("t", out JsonElement eventElement))
            {
                return Enum.Parse<Events>(eventElement.GetString() ?? throw new Exception("Invalid Payload"));
            }

            throw new JsonException("Invalid Payload");
        }
    }
}
