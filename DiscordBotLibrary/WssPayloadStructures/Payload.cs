namespace DiscordBotLibrary.WssPayloadStructures
{
    internal sealed record Payload<T>
    {
        public OpCode Op { get; init; }

        [JsonProperty("d")]
        public T Data { get; init; }

        public Payload(OpCode op, T data)
        {
            Op = op;
            Data = data;
        }
    }
}
