namespace DiscordBotLibrary.ChannelResources
{
    /// <summary>
    /// Represents the type of a Discord channel.
    /// </summary>
    public enum ChannelType
    {
        /// <summary>
        /// A text channel within a server.
        /// </summary>
        GuildText = 0,

        /// <summary>
        /// A direct message between users.
        /// </summary>
        DM = 1,

        /// <summary>
        /// A voice channel within a server.
        /// </summary>
        GuildVoice = 2,

        /// <summary>
        /// A direct message between multiple users.
        /// </summary>
        GroupDM = 3,

        /// <summary>
        /// An organizational category that contains up to 50 channels.
        /// </summary>
        GuildCategory = 4,

        /// <summary>
        /// A channel that users can follow and crosspost into their own server (formerly news channels).
        /// </summary>
        GuildAnnouncement = 5,

        /// <summary>
        /// A temporary sub-channel within a GuildAnnouncement channel.
        /// </summary>
        AnnouncementThread = 10,

        /// <summary>
        /// A temporary sub-channel within a GuildText or GuildForum channel.
        /// </summary>
        PublicThread = 11,

        /// <summary>
        /// A temporary sub-channel within a GuildText channel that is only viewable by those invited and those with the MANAGE_THREADS permission.
        /// </summary>
        PrivateThread = 12,

        /// <summary>
        /// A voice channel for hosting events with an audience.
        /// </summary>
        GuildStageVoice = 13,

        /// <summary>
        /// The channel in a hub containing the listed servers.
        /// </summary>
        GuildDirectory = 14,

        /// <summary>
        /// A channel that can only contain threads.
        /// </summary>
        GuildForum = 15,

        /// <summary>
        /// A channel that can only contain threads, similar to GuildForum channels.
        /// </summary>
        GuildMedia = 16
    }
}