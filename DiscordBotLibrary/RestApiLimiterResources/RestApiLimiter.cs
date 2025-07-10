using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace DiscordBotLibrary.RestApiLimiterResources
{
    internal sealed partial class RestApiLimiter
    {
        private readonly ConcurrentDictionary<string, string> _routeToBucketId = new();
        private readonly ConcurrentDictionary<string, HttpRateLimitInfo> _rateLimitInfo = new();
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _routeLocks = new();

        private bool IsGlobalTimeout => _globalTimeout != TimeSpan.Zero;
        private TimeSpan _globalTimeout = TimeSpan.Zero;
        private readonly HttpClient _httpClient;

        public RestApiLimiter(DiscordClientConfig clientConfig)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://discord.com/api/v{clientConfig.Version}/"),
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", clientConfig.Token);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CacxCord (https://github.com/Cristiano3120/DiscordBotLibrary , v1.0)");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T?> GetAsync<T>(string endpoint, CallerInfos callerInfos)
            => await HandleRequestAsync<T, T>(HttpRequestType.Get, default!, callerInfos, endpoint);

        public async Task<bool> DeleteAsync(string endpoint, CallerInfos callerInfos)
            => await HandleRequestAsync<bool, bool>(HttpRequestType.Delete, default!, callerInfos, endpoint);

        public async Task<TOutput?> PostAsync<TInput, TOutput>(TInput content, string endpoint, CallerInfos callerInfos)
            => await HandleRequestAsync<TInput, TOutput>(HttpRequestType.Post, content, callerInfos, endpoint);

        public async Task<TOutput?> PatchAsync<TInput, TOutput>(TInput content, string endpoint, CallerInfos callerInfos)
            => await HandleRequestAsync<TInput, TOutput>(HttpRequestType.Patch, content, callerInfos, endpoint);

        public async Task<string> GetStringAsync(string endpoint)
            => await _httpClient.GetStringAsync(endpoint);

        private async Task<TOutput?> HandleRequestAsync<TInput, TOutput>(HttpRequestType httpRequestType, TInput input, CallerInfos callerInfos,
            string endpoint)
        {
            SemaphoreSlim? semaphoreSlim = null;
            try
            {
                while (true)
                {
                    DiscordClient.Logger.Log(LogLevel.Debug, $"[{httpRequestType}]: requesting {endpoint}");
                    string formattedEndpoint = FormatEndpoint(endpoint);

                    semaphoreSlim = await WaitIfNeededAsync(formattedEndpoint);

                    HttpResponseMessage response = httpRequestType switch
                    {
                        HttpRequestType.Get => await _httpClient.GetAsync(endpoint),
                        HttpRequestType.Delete => await _httpClient.DeleteAsync(endpoint),
                        HttpRequestType.Post => await _httpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(input, DiscordClient.SendJsonSerializerSettings), Encoding.UTF8, "application/json")),
                        HttpRequestType.Patch => await _httpClient.PatchAsync(endpoint, new StringContent(JsonConvert.SerializeObject(input, DiscordClient.SendJsonSerializerSettings), Encoding.UTF8, "application/json")),
                        _ => throw new InvalidEnumArgumentException("This method only allows HttpRequestType.Get, HttpRequestType.Delete, HttpRequestType.Post or HttpRequestType.Patch"),
                    };

                    string json = await response.Content.ReadAsStringAsync();
                    JToken jsonToken = JToken.Parse(json);
                    string prettyJson = jsonToken.ToString(Formatting.Indented);

                    if (response.IsSuccessStatusCode)
                    {
                        DiscordClient.Logger.LogHttpPayload(PayloadType.Received, httpRequestType, json);
                        HttpRateLimitInfo newInfo = GetRateLimitInfo(response.Headers);
                        if (!string.IsNullOrEmpty(newInfo.BucketId))
                        {
                            _rateLimitInfo[newInfo.BucketId] = newInfo;
                            _routeToBucketId[formattedEndpoint] = newInfo.BucketId;
                        }

                        //TOutput is bool when deleting a resource
                        if (typeof(TOutput) == typeof(bool))
                            return (TOutput)(object)true;

                        TOutput? result = JsonConvert.DeserializeObject<TOutput>(json, DiscordClient.ReceiveJsonSerializerOptions);
                        return result ?? throw new InvalidOperationException($"Deserialization returned null." +
                            $" Response: {json} \n (expected: {typeof(TOutput)})");
                    }

                    DiscordClient.Logger.LogError($"While sending a [{httpRequestType}] request to the endpoint: {endpoint}", callerInfos);
                    if (!await HandleErrorCode(response, callerInfos, endpoint, (int)response.StatusCode))
                    {
                        DiscordClient.Logger.CustomLog(ConsoleColor.Red, LogLevel.Error, $"[API RESPONSE]: {prettyJson}\n");
                        return default;
                    }
                }
            }
            finally
            {
                semaphoreSlim?.Release();
            }
        }

        #region HandleErrorCode

        /// <summary>
        /// True if the error is handable. If false cancel the method
        /// </summary>
        private async Task<bool> HandleErrorCode(HttpResponseMessage response, CallerInfos callerInfos, string endpoint, int statusCode)
        {
            DiscordClient.Logger.CustomLog(ConsoleColor.Red, LogLevel.Error, $"[API RESPONSE]: {response.StatusCode} - {response.ReasonPhrase}");
            switch (statusCode)
            {
                case 400:
                    DiscordClient.Logger.LogError($"The data you sent is invalid in some form", callerInfos);
                    return false;
                case 403:
                    DiscordClient.Logger.LogError($"You do not have the permissions to execute that", callerInfos);
                    return false;
                case 404:
                    DiscordClient.Logger.LogError($"The resource you are trying to access doesn´t exist", callerInfos);
                    return false;
                case 429:
                    await HandleError429(response, endpoint);
                    return true;
                default:
                    throw new HttpRequestException($"[HTTP ERROR]: {(int)response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        private async Task HandleError429(HttpResponseMessage response, string endpoint)
        {
            DiscordClient.Logger.Log(LogLevel.Debug, $"[RateLimit]: while requesting {endpoint}");

            if (CheckIfGlobal(await response.Content.ReadAsStringAsync(), out TimeSpan timeToWait))
            {
                DiscordClient.Logger.Log(LogLevel.Warning, $"[GlobalRateLimit]: Blocked for {timeToWait.TotalSeconds:F2}s");

                _globalTimeout = timeToWait;
                await Task.Delay(timeToWait);
                _globalTimeout = TimeSpan.Zero;
            }

            HttpRateLimitInfo retryInfo = GetRateLimitInfo(response.Headers);
            TimeSpan retryDelay = retryInfo.RetryAfter
                ?? (retryInfo.ResetAt.HasValue
                    ? retryInfo.ResetAt.Value - DateTimeOffset.UtcNow
                    : TimeSpan.FromSeconds(5));

            if (retryDelay > TimeSpan.Zero)
            {
                DiscordClient.Logger.Log(LogLevel.Debug, $"[RateLimit]: Retrying in {retryDelay.TotalSeconds:F2}s");
                await Task.Delay(retryDelay);
            }
        }

        #endregion

        private async Task<SemaphoreSlim> WaitIfNeededAsync(string endpoint)
        {
            if (IsGlobalTimeout)
            {
                DiscordClient.Logger.Log(LogLevel.Debug, $"[GlobalRateLimit] waiting {_globalTimeout:F2}s");
                await Task.Delay(_globalTimeout);
            }

            if (_routeToBucketId.TryGetValue(endpoint, out string? bucketId) &&
                _rateLimitInfo.TryGetValue(bucketId, out HttpRateLimitInfo? cachedInfo))
            {
                if (cachedInfo.Remaining == 0)
                {
                    TimeSpan delay = cachedInfo.RetryAfter
                        ?? (cachedInfo.ResetAt.HasValue
                            ? cachedInfo.ResetAt.Value - DateTimeOffset.UtcNow
                            : TimeSpan.Zero);

                    if (delay > TimeSpan.Zero)
                    {
                        DiscordClient.Logger.Log(LogLevel.Debug, $"[RateLimit] waiting {delay.TotalSeconds:F2}s for bucket {bucketId}");
                        await Task.Delay(delay);
                    }
                }
            }

            SemaphoreSlim semaphoreSlim = _routeLocks.GetOrAdd(endpoint, _ => new SemaphoreSlim(1, 1));
            await semaphoreSlim.WaitAsync();
            return semaphoreSlim;
        }

        private static bool CheckIfGlobal(string content, out TimeSpan timeToWait)
        {
            JToken jToken = JToken.Parse(content);
            timeToWait = TimeSpan.Zero;

            if (jToken.TryGetProperty("global", out JToken? global) && global.Type == JTokenType.Boolean && global.Value<bool>())
            {
                if (jToken.TryGetProperty("retry_after", out JToken? retry))
                {
                    double seconds;

                    if (retry.Type == JTokenType.Integer || retry.Type == JTokenType.Float)
                    {
                        seconds = retry.Value<double>();
                    }
                    else if (retry.Type == JTokenType.String)
                    {
                        seconds = double.Parse(retry.Value<string>()!, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        throw new Exception("Unexpected token type for 'retry_after'");
                    }

                    timeToWait = TimeSpan.FromSeconds(seconds);
                }
            }

            return timeToWait != TimeSpan.Zero;
        }

        private static HttpRateLimitInfo GetRateLimitInfo(HttpResponseHeaders headers)
        {
            HttpRateLimitInfo info = new();

            if (headers.TryGetValues("X-RateLimit-Bucket", out IEnumerable<string>? bucket))
                info.BucketId = bucket.FirstOrDefault();

            if (headers.TryGetValues("X-RateLimit-Limit", out IEnumerable<string>? limit) && int.TryParse(limit.FirstOrDefault(), out var limitValue))
                info.Limit = limitValue;

            if (headers.TryGetValues("X-RateLimit-Remaining", out IEnumerable<string>? remaining) && int.TryParse(remaining.FirstOrDefault(), out var remainingValue))
                info.Remaining = remainingValue;

            if (headers.TryGetValues("X-RateLimit-Reset", out IEnumerable<string>? reset) && double.TryParse(reset.FirstOrDefault(), CultureInfo.InvariantCulture, out var resetUnix))
                info.ResetAt = DateTimeOffset.FromUnixTimeSeconds((long)resetUnix);

            if (headers.TryGetValues("Retry-After", out IEnumerable<string>? retry))
            {
                string? retryStr = retry.FirstOrDefault();
                if (int.TryParse(retryStr, out int seconds))
                    info.RetryAfter = TimeSpan.FromSeconds(seconds);
                else if (double.TryParse(retryStr, out double milliseconds))
                    info.RetryAfter = TimeSpan.FromMilliseconds(milliseconds);
            }

            return info;
        }

        private static string FormatEndpoint(string endpoint)
        {
            string standardized = GenerateIdRegex().Replace(endpoint, ":id");
            int queryIndex = standardized.IndexOf('?');
            if (queryIndex >= 0)
                standardized = standardized[..queryIndex];

            return standardized.ToLowerInvariant();
        }

        [GeneratedRegex(@"\d{16,19}", RegexOptions.Compiled)]
        private static partial Regex GenerateIdRegex();
    }
}