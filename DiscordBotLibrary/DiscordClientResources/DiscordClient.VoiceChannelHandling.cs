namespace DiscordBotLibrary.DiscordClientResources
{
    public sealed partial class DiscordClient
    {
        internal void ReceivedVoiceServerUpdate(VoiceServerUpdate voiceServerUpdate)
            => VoiceChannelHandler!.ReceivedVoiceServerUpdate(voiceServerUpdate);
    }
}
