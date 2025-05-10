using System.Runtime.Serialization;

namespace DiscordBotLibrary
{
    /// <summary>
    /// OAuth2 scopes supported by Discord.
    /// </summary>
    public enum OAuth2Scope : byte
    {
        [EnumMember(Value = "activities.read")]
        ActivitiesRead,

        [EnumMember(Value = "activities.write")]
        ActivitiesWrite,

        [EnumMember(Value = "applications.builds.read")]
        ApplicationsBuildsRead,

        [EnumMember(Value = "applications.builds.upload")]
        ApplicationsBuildsUpload,

        [EnumMember(Value = "applications.commands")]
        ApplicationsCommands,

        [EnumMember(Value = "applications.commands.update")]
        ApplicationsCommandsUpdate,

        [EnumMember(Value = "applications.commands.permissions.update")]
        ApplicationsCommandsPermissionsUpdate,

        [EnumMember(Value = "applications.entitlements")]
        ApplicationsEntitlements,

        [EnumMember(Value = "applications.store.update")]
        ApplicationsStoreUpdate,

        [EnumMember(Value = "bot")]
        Bot,

        [EnumMember(Value = "connections")]
        Connections,

        [EnumMember(Value = "dm_channels.read")]
        DmChannelsRead,

        [EnumMember(Value = "email")]
        Email,

        [EnumMember(Value = "gdm.join")]
        GdmJoin,

        [EnumMember(Value = "guilds")]
        Guilds,

        [EnumMember(Value = "guilds.join")]
        GuildsJoin,

        [EnumMember(Value = "guilds.members.read")]
        GuildsMembersRead,

        [EnumMember(Value = "identify")]
        Identify,

        [EnumMember(Value = "messages.read")]
        MessagesRead,

        [EnumMember(Value = "relationships.read")]
        RelationshipsRead,

        [EnumMember(Value = "role_connections.write")]
        RoleConnectionsWrite,

        [EnumMember(Value = "rpc")]
        Rpc,

        [EnumMember(Value = "rpc.activities.write")]
        RpcActivitiesWrite,

        [EnumMember(Value = "rpc.notifications.read")]
        RpcNotificationsRead,

        [EnumMember(Value = "rpc.voice.read")]
        RpcVoiceRead,

        [EnumMember(Value = "rpc.voice.write")]
        RpcVoiceWrite,

        [EnumMember(Value = "voice")]
        Voice,

        [EnumMember(Value = "webhook.incoming")]
        WebhookIncoming
    }
}