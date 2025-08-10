namespace DiscordBotLibrary.ChannelResources.PartialChannel
{
    public sealed partial record Channel
    {
        /// <summary>
        /// <see cref="ChannelType.Text"/> and <see cref="ChannelType.Announcement"/> channels can create threads from messages.
        /// </summary>
        public async Task<Channel?> CreateThreadFromMessageAsync(StartThreadFromMessageParams startThreadParams, ulong messageId)
        {
            CallerInfos callerInfos = CallerInfos.Create();
            if (Type is not ChannelType.Text or ChannelType.Announcement)
            {
                string errorMsg = "You can only create threads from messages in text or announcement channels.";
                DiscordClient.Logger.LogError(errorMsg, callerInfos);
                return null;
            }

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Messages, $"{messageId}/threads");
            return await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .PostAsync<StartThreadFromMessageParams, Channel>(startThreadParams, endpoint, callerInfos);
        }

        /// <summary>
        /// Creates a new thread. This overload only works for <see cref="ChannelType.Text"/>
        /// and <see cref="ChannelType.Announcement"/> channels.
        /// <para>
        /// For <see cref="ChannelType.Media"/> and <see cref="ChannelType.Forum"/> channels, use the <see cref="CreateThreadAsync(StartThreadParams)"/> overload instead.
        ///</para>
        /// </summary>
        /// <param name="startThreadParams"></param>
        /// <returns></returns>
        public async Task<Channel?> CreateThreadAsync(StartThreadParams startThreadParams)
        {
            CallerInfos callerInfos = CallerInfos.Create();

            if (Type is not (ChannelType.Text or ChannelType.Announcement))
            {
                DiscordClient.Logger.LogError("You can only create threads in text based channels", callerInfos);
                return null;
            }

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Threads);
            return await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .PostAsync<StartThreadParams, Channel>(startThreadParams, endpoint, callerInfos);
        }

        public async Task<bool> JoinThreadAsync()
        {
            CallerInfos callerInfos = CallerInfos.Create();
            Logger logger = DiscordClient.Logger;

            if (!IsThread(callerInfos))
            {
                logger.LogError("You can only join threads", callerInfos);
                return false;
            }

            if (ThreadMetadata!.Value.Archived)
            {
                logger.LogError("You can´t join an archived thread", callerInfos);
                return false;
            }

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Thread_Members, "@me");
            return await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .PutAsync<object?, bool>(null, endpoint, callerInfos);
        }

        public async Task<bool> AddThreadMemberAsync(ulong userId)
        {
            CallerInfos callerInfos = CallerInfos.Create();
            Logger logger = DiscordClient.Logger;

            if (!IsThread(callerInfos))
            {
                logger.LogError("You can only add thread members to threads", callerInfos);
                return false;
            }

            if (!CheckPermissions(DiscordPermissions.SendMessagesInThreads))
            {
                logger.LogError("You need send messages permissions to add a member to thread", callerInfos);
                return false;
            }

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Thread_Members, $"{userId}");
            return await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .PutAsync<object?, bool>(null, endpoint, callerInfos);
        }

        public async Task<bool> LeaveThreadAsync()
        {
            CallerInfos callerInfos = CallerInfos.Create();
            Logger logger = DiscordClient.Logger;

            if (!IsThread(callerInfos))
            {
                logger.LogError("You can only leave threads", callerInfos);
                return false;
            }

            if (ThreadMetadata!.Value.Archived)
            {
                logger.LogError("You can´t leave an archived thread", callerInfos);
                return false;
            }

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Thread_Members, "@me");
            return await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .DeleteAsync(endpoint, callerInfos);
        }

        public async Task<bool> RemoveThreadMemberAsync(ulong userId)
        {
            DiscordClient client = DiscordClient.GetDiscordClient();
            CallerInfos callerInfos = CallerInfos.Create();
            Logger logger = DiscordClient.Logger;

            if (!IsThread(callerInfos))
            {
                logger.LogError("You can only remove thread members from threads", callerInfos);
                return false;
            }

            if (!CheckPermissions(DiscordPermissions.ManageThreads) && Type is not ChannelType.PrivateThread)
            {
                logger.LogError("You need manage threads permissions to remove a member from thread", callerInfos);
                return false;
            }

            if (Type is ChannelType.PrivateThread && OwnerId != client.CurrentUser?.Id)
            {
                logger.LogError("You can only remove members from private threads if you are the owner of the thread", callerInfos);
                return false;
            }

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Thread_Members, $"{userId}");
            return await client.RestApiLimiter.DeleteAsync(endpoint, callerInfos);
        }

        /// <summary>
        /// Gets a thread member by user ID. 
        /// <para>If <paramref name="getMemberInfo"/> is true, it will also return the GuildMember information.</para>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="getMemberInfo"></param>
        /// <returns></returns>
        public async Task<ThreadMember?> GetThreadMemberAsync(ulong userId, bool getMemberInfo = true)
        {
            CallerInfos callerInfos = CallerInfos.Create();
            if (!IsThread(callerInfos))
            {
                DiscordClient.Logger.LogError("You can only get thread members from threads", callerInfos);
                return null;
            }

            Dictionary<string, string> queryParams = new()
            {
                { "with_member", getMemberInfo.ToString() }
            };

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Thread_Members, $"{userId}", queryParams);
            return await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .GetAsync<ThreadMember>(endpoint, callerInfos);
        }

        /// <summary>
        /// <paramref name="count"/> is the maximum number of members to return, default is 100. 
        /// <br></br>Setting it to 0 will return all members.
        /// </summary>
        /// <returns></returns>
        public async Task<ThreadMember[]?> GetThreadMembersAsync(bool getMemberInfo = true, byte count = 100)
        {
            DiscordClient client = DiscordClient.GetDiscordClient();
            CallerInfos callerInfos = CallerInfos.Create();

            if (!client.IntentChecker(callerInfos, Intents.GuildMembers))
            {
                return null;
            }

            if (!IsThread(callerInfos))
            {
                DiscordClient.Logger.LogError("You can only get thread members from threads", callerInfos);
                return null;
            }

            if (count > 100)
                count = 100;

            Dictionary<string, string> queryParams = new()
            {
                { "with_member", getMemberInfo.ToString().ToLower() },
                { "limit", count.ToString() }
            };

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Thread_Members, null, queryParams);

            if (count > 0)
            {
                return await client
                    .RestApiLimiter
                    .GetAsync<ThreadMember[]>(endpoint, callerInfos);
            }

            return await GetAllThreadMembersAsync(getMemberInfo, callerInfos);
        }

        private async Task<ThreadMember[]?> GetAllThreadMembersAsync(bool getMemberInfo, CallerInfos callerInfos)
        {
            List<ThreadMember> members = new(MemberCount ?? 10);
            int lastReturnedCount;

            do
            {
                Dictionary<string, string> queryParams = new()
                {
                    { "with_member", getMemberInfo.ToString().ToLower() },
                    { "limit", "100" }
                };
                if (members.Count > 0)
                {
                    queryParams["after"] = members.Last().UserId?.ToString() ?? "0";
                }

                string endpoint = RestApiEndpoints.GetChannelEndpoint(
                    Id, ChannelSubresource.Thread_Members, null, queryParams);

                DiscordClient.Logger.Log(LogLevel.Debug, $"Getting thread members from {endpoint} with getMemberInfo={getMemberInfo}");
                
                IEnumerable<ThreadMember>? paginatedMembers = await DiscordClient
                    .GetDiscordClient()
                    .RestApiLimiter
                    .GetAsync<IEnumerable<ThreadMember>>(endpoint, callerInfos);

                if (paginatedMembers is null)
                {
                    DiscordClient.Logger.LogError("Failed to get thread members", callerInfos);
                    return null;
                }

                members.AddRange(paginatedMembers);
                lastReturnedCount = paginatedMembers.Count();
            }
            while (lastReturnedCount == 100);

            return [.. members];
        }

        /// <summary>
        /// Returns archived threads in the channel that are public. <para></para>
        /// When called on a GUILD_TEXT channel, returns threads of type PUBLIC_THREAD. <para></para>
        /// When called on a GUILD_ANNOUNCEMENT channel returns threads of type ANNOUNCEMENT_THREAD. <para></para>
        /// Threads are ordered by archive_timestamp, in descending order.
        /// 
        /// <para>
        /// <paramref name="before"/>: Gets all archived threads before the specified time. <br></br>
        /// <paramref name="limit"/>: The maximum number of threads to return.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public async Task<ArchivedThreadsResponse?> GetPublicArchivedThreadsAsync(DateTimeOffset? before = null, int? limit = null)
            => await GetArchivedThreadsAsync(ChannelSubresource.Threads, "archived/public", 
                CallerInfos.Create(), before, limit);

        /// <summary>
        /// Returns archived threads in the channel that are of type PRIVATE_THREAD. <br></br>
        /// Threads are ordered by archive_timestamp, in descending order. <br></br>
        /// Requires both the READ_MESSAGE_HISTORY and MANAGE_THREADS permissions.
        /// 
        /// <para>
        /// <paramref name="before"/>: Gets all archived threads before the specified time. <br></br>
        /// <paramref name="limit"/>: The maximum number of threads to return.
        /// </para>
        /// </summary>
        public async Task<ArchivedThreadsResponse?> GetPrivateArchivedThreadsAsync(DateTimeOffset? before = null, int? limit = null)
        {
            CallerInfos callerInfos = CallerInfos.Create();
            if (!CheckPermissions(DiscordPermissions.ManageThreads))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageThreads), callerInfos);
                return null;
            }

            return await GetArchivedThreadsAsync(ChannelSubresource.Threads, "archived/private", 
                callerInfos, before, limit);
        }

        public async Task<ArchivedThreadsResponse?> GetJoinedPrivateArchivedThreads(DateTimeOffset? before = null, int? limit = null)
            => await GetArchivedThreadsAsync(ChannelSubresource.Users, $"users/@me/threads/archived/private", 
                CallerInfos.Create(), before, limit);

        private async Task<ArchivedThreadsResponse?> GetArchivedThreadsAsync(
            ChannelSubresource subresource, 
            string dynamicEndpointPart,
            CallerInfos callerInfos,
            DateTimeOffset? before = null, 
            int? limit = null)
        {
            if (!IsTextBased(callerInfos))
            {
                return null;
            }

            if (!CheckPermissions(DiscordPermissions.ReadMessageHistory))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.ReadMessageHistory), callerInfos);
                return null;
            }

            if (limit <= 0)
            {
                DiscordClient.Logger.Log(LogLevel.Info, "Set limit to null cause it was <= 0");
                limit = null;
            }

            Dictionary<string, string> queryParams = new();
            if (before.HasValue)
            {
                queryParams["before"] = before.Value.ToString();
            }
            if (limit.HasValue)
            {
                queryParams["limit"] = limit.Value.ToString();
            }

            string endpoint = RestApiEndpoints
                .GetChannelEndpoint(Id, subresource, dynamicEndpointPart, queryParams);

            return await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .GetAsync<ArchivedThreadsResponse>(endpoint, callerInfos);
        }
    }
}
