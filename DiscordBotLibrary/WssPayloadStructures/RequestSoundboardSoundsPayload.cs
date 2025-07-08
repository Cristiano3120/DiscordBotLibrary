namespace DiscordBotLibrary.WssPayloadStructures
{
    internal sealed record RequestSoundboardSoundsPayload
    {
        public string[] GuildIds { get; init; }

        public RequestSoundboardSoundsPayload(string[] guildIds)
        {
            GuildIds = guildIds;
        }
    }
}
