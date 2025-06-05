namespace DiscordBotLibrary.RequestSoundboardResources
{
    internal sealed class SoundboardSounds
    {
        /// <summary>
        /// TYPE: Snowflake  
        /// ID of the guild
        /// </summary>
        [JsonPropertyName("guild_id")]
        public ulong GuildId { get; set; }

        /// <summary>
        /// The guild's soundboard sounds
        /// </summary>
        [JsonPropertyName("soundboard_sounds")]
        public SoundboardSound[] SoundboardSoundsArr { get; set; } = [];
    }
}
