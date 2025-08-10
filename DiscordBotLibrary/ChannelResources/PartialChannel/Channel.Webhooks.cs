namespace DiscordBotLibrary.ChannelResources.PartialChannel
{
    public sealed partial record Channel
    {
        #region GetWebhooks

        public async Task<Webhook[]?> GetWebhooksAsync()
        {
            CallerInfos callerInfos = CallerInfos.Create();
            if (!CheckPermissions(DiscordPermissions.ManageWebhooks))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageWebhooks), callerInfos);
                return null;
            }

            string getEndpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Webhooks);
            return await DiscordClient.GetDiscordClient().RestApiLimiter.GetAsync<Webhook[]?>(getEndpoint, callerInfos);
        }

        public async Task<Webhook[]?> GetIncomingWebhooksAsync()
        {
            Webhook[]? webhooks = await GetWebhooksAsync();
            return webhooks is null
                ? null
                : [..webhooks.Where(x => x.Type == WebhookType.Incoming)];
        }

        public async Task<Webhook[]?> GetChannelFollowerWebhooksAsync()
        {
            Webhook[]? webhooks = await GetWebhooksAsync();
            return webhooks is null
                ? null
                : [.. webhooks.Where(x => x.Type == WebhookType.ChannelFollower)];
        }

        #endregion

        #region DeleteWebhooks

        /// <summary>
        /// <see cref="DiscordPermissions.ManageWebhooks"/> are needed.
        /// </summary>
        /// <param name="webhookType"></param>
        public async Task<bool> DeleteAllWebhooksAsync()
        {
            if (!CheckPermissions(DiscordPermissions.ManageWebhooks))
            {
                CallerInfos callerInfos = CallerInfos.Create();
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageWebhooks), callerInfos);
                return false;
            }

            Webhook[]? webhooks = await GetWebhooksAsync();
            if (webhooks is null)
            {
                return false;
            }

            foreach (Webhook webhook in webhooks)
            {
                if (webhook.Token is not null)
                {
                    await DeleteWebhookAsync(webhook.Id, webhook.Token);
                }

                await DeleteWebhookAsync(webhook.Id);
            }
        
            return true;
        }

        /// <summary>
        /// <see cref="DiscordPermissions.ManageWebhooks"/> are needed.<br></br>
        /// <see cref="WebhookType.Application"/> can never be part of an channel
        /// </summary>
        /// <param name="webhookType"></param>
        public async Task<bool> DeleteSpecificWebhooksAsync(WebhookType webhookType)
        {
            if (!CheckPermissions(DiscordPermissions.ManageWebhooks))
            {
                CallerInfos callerInfos = CallerInfos.Create();
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageWebhooks), callerInfos);
                return false;
            }

            Webhook[]? webhooks = await GetWebhooksAsync();
            if (webhooks is null)
            {
                return false;
            }

            foreach (Webhook webhook in webhooks)
            {
                if (webhookType != webhook.Type)
                    continue;
                
                if (webhook.Token is not null)
                {
                    await DeleteWebhookAsync(webhook.Id, webhook.Token);
                }
                else
                {
                    await DeleteWebhookAsync(webhook.Id);
                }
            }

            return true;
        }

        /// <summary>
        /// Delete a webhook permanently. Requires the MANAGE_WEBHOOKS permission. <br></br>
        /// Fires a Webhooks Update event.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteWebhookAsync(ulong webhookId)
        {
            CallerInfos callerInfos = CallerInfos.Create();
            if (!CheckPermissions(DiscordPermissions.ManageWebhooks))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageWebhooks), callerInfos);
                return false;
            }

            string endpoint = RestApiEndpoints.GetWebhookEndpoint(webhookId, HttpRequestType.Delete);
            return await DiscordClient.GetDiscordClient().RestApiLimiter.DeleteAsync(endpoint, CallerInfos.Create());
        }

        /// <summary>
        /// Same as <see cref="DeleteWebhookAsync(ulong)"/> except this call does not require authentication.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DeleteWebhookAsync(ulong webhookId, string token)
        {
            CallerInfos callerInfos = CallerInfos.Create();
            if (!CheckPermissions(DiscordPermissions.ManageWebhooks))
            {
                DiscordClient.Logger.LogError(GetMissingPermissionsErrorMsg(DiscordPermissions.ManageWebhooks), callerInfos);
                return false;
            }

            string endpoint = RestApiEndpoints.GetWebhookEndpoint(webhookId, HttpRequestType.Delete, token);
            return await DiscordClient.GetDiscordClient().RestApiLimiter.DeleteAsync(endpoint, callerInfos);
        }

        #endregion

        public async Task<Webhook?> CreateWebhook(string name, byte[]? imageBytes)
        {
            CallerInfos callerInfos = CallerInfos.Create();
            if (!Webhook.CheckWebhookName(name))
            {
                string errorMsg = $"Webhook name invalid!.";
                DiscordClient.Logger.LogError(errorMsg, callerInfos);
                return null;
            }

            const int MiB = 1024 * 1024;
            const int maxImageSize = 10 * MiB;
            string? imageBase64 = null;

            if (imageBytes is not null)
            {
                if (imageBytes.Length > maxImageSize)
                {
                    string errorMsg = $"Webhook image size exceeds the maximum limit of 10 MiB";
                    DiscordClient.Logger.LogError(errorMsg, callerInfos);
                    return null;
                }

                imageBase64 = Convert.ToBase64String(imageBytes);
            }

            CreateWebhookData createWebhookData = new()
            {
                Name = name,
                ImageBase64 = imageBase64
            };

            string endpoint = RestApiEndpoints.GetChannelEndpoint(Id, ChannelSubresource.Webhooks);
            return await DiscordClient
                .GetDiscordClient()
                .RestApiLimiter
                .PostAsync<CreateWebhookData, Webhook>(createWebhookData, endpoint, callerInfos);
        }
    }
}
