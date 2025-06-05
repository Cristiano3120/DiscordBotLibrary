namespace DiscordBotLibrary.RequestGuildMembersResources
{
    internal sealed record RequestGuildMembersCache
    {
        public TaskCompletionSource<List<GuildMember>> TaskCompletionSource { get; init; }
        public RequestGuildMembers RequestGuildMembers { get; init; }
        public List<GuildMember> GuildMembers { get; set; }
        public bool Cache { get; init; } = true;

        public RequestGuildMembersCache(TaskCompletionSource<List<GuildMember>> tsc
            , RequestGuildMembers requestGuildMembers, bool cache)
        {
            TaskCompletionSource = tsc;
            GuildMembers = new List<GuildMember>();
            RequestGuildMembers = requestGuildMembers;
            Cache = cache;
        }
    }
}
