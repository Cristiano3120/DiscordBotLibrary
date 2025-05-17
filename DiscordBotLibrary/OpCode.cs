namespace DiscordBotLibrary
{
    internal enum OpCode : byte
    {
        /// <summary>
        /// An event was dispatched. (z. B. MESSAGE_CREATE, READY, etc.)
        /// </summary>
        Dispatch = 0,

        /// <summary>
        /// Used to ping the server to ensure the connection is alive.
        /// </summary>
        Heartbeat = 1,

        /// <summary>
        /// Starts a new session during the initial handshake.
        /// </summary>
        Identify = 2,

        /// <summary>
        /// Used to update the client's presence.
        /// </summary>
        PresenceUpdate = 3,

        /// <summary>
        /// Used to join/leave or move between voice channels.
        /// </summary>
        VoiceStateUpdate = 4,

        /// <summary>
        /// Resume a previous session that was disconnected.
        /// </summary>
        Resume = 6,

        /// <summary>
        /// You should reconnect to the gateway (and resume if possible).
        /// </summary>
        Reconnect = 7,

        /// <summary>
        /// Request information about offline guild members.
        /// </summary>
        RequestGuildMembers = 8,

        /// <summary>
        /// The client is acknowledging a received event.
        /// </summary>
        InvalidSession = 9,

        /// <summary>
        /// Sent immediately after connecting, contains the heartbeat interval.
        /// </summary>
        Hello = 10,

        /// <summary>
        /// Sent in response to receiving a heartbeat to acknowledge that it has been received.
        /// </summary>
        HeartbeatAck = 11,

        /// <summary>
        /// Request information about soundboard sounds in a set of guilds.
        /// </summary>
        RequestSoundboardSounds = 31,
    }
}
