﻿namespace DiscordBotLibrary.ChannelResources.ChannelEnums
{
    /// <summary>
    /// Represents the type of a Discord channel.
    /// </summary>
    public enum ChannelType : byte
    {
        /// <summary>
        /// A text channel within a server.
        /// </summary>
        Text = 0,

        /// <summary>
        /// A direct message between users.
        /// </summary>
        DM = 1,

        /// <summary>
        /// A voice channel within a server.
        /// </summary>
        Voice = 2,

        /// <summary>
        /// A direct message between multiple users.
        /// </summary>
        GroupDM = 3,

        /// <summary>
        /// An organizational category that contains up to 50 channels.
        /// </summary>
        Category = 4,

        /// <summary>
        /// A channel that users can follow and crosspost into their own server (formerly news channels).
        /// </summary>
        Announcement = 5,

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
        StageVoice = 13,

        /// <summary>
        /// The channel in a hub containing the listed servers.
        /// </summary>
        Directory = 14,

        /// <summary>
        /// A channel that can only contain threads.
        /// </summary>
        Forum = 15,

        /// <summary>
        /// A channel that can only contain threads, similar to GuildForum channels.
        /// </summary>
        Media = 16
    }
}