namespace DiscordBotLibrary.RequestSoundboardResources
{
    internal sealed class SoundboardSounds
    {
        /// <summary>
        /// TYPE: Snowflake  
        /// ID of the guild
        /// </summary>
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }

        /// <summary>
        /// The guild's soundboard sounds
        /// </summary>
        [JsonProperty("soundboard_sounds")]
        public SoundboardSound[] SoundboardSoundsArr { get; set; } = [];
    }
}
