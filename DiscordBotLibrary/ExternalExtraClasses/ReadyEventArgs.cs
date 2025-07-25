﻿namespace DiscordBotLibrary.ExternalExtraClasses
{
    /// <summary>
    /// Represents the all the data from al the shards after each of them is ready.
    /// </summary>
    public sealed class ReadyEventArgs
    {
        /// <summary>
        /// Information about the connected user including email.
        /// </summary>
        [JsonProperty("user")]
        public User? DiscordUser { get; init; }

        /// <summary>
        /// Guilds the user is in (sent as UnavailableGuild objects).
        /// </summary>
        [JsonProperty("guilds")]
        public UnavailableGuild[] Guilds { get; set; } = [];

        /// <summary>
        /// Contains infos about the application
        /// </summary>
        [JsonProperty("application")]
        public Application? Application { get; init; }
    }
}
