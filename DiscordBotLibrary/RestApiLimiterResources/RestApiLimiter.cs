using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using DiscordBotLibrary.MessageResources;

namespace DiscordBotLibrary.RestApiLimiterResources
{
    internal sealed partial class RestApiLimiter(HttpClient httpClient)
    {
        private readonly ConcurrentDictionary<string, string> _routeToBucketId = new();
        private readonly ConcurrentDictionary<string, HttpRateLimitInfo> _rateLimitInfo = new();
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _routeLocks = new();

        private readonly HttpClient _httpClient = httpClient;
        private TimeSpan _globalTimeout = TimeSpan.Zero;
        private bool IsGlobalTimeout => _globalTimeout != TimeSpan.Zero;

        #region External methods

        public async Task<T?> GetAsync<T>(string endpoint, CallerInfos callerInfos)
        {
            SemaphoreSlim semaphoreSlim = default!;
            try
            {
                DiscordClient.Logger.LogDebug($"[GetAsync<T>(string endpoint)] requesting {endpoint}");
                string formattedEndpoint = FormatEndpoint(endpoint);

                semaphoreSlim = await WaitIfNeededAsync(formattedEndpoint);
                HttpResponseMessage? response = await GetDeleteRequestAsync(HttpRequestType.Get, endpoint, callerInfos);
                if (response is null)
                    return default;

                HttpRateLimitInfo newInfo = GetRateLimitInfo(response.Headers);

                if (!string.IsNullOrEmpty(newInfo.BucketId))
                {
                    _rateLimitInfo[newInfo.BucketId] = newInfo;
                    _routeToBucketId[formattedEndpoint] = newInfo.BucketId;
                }

                string json = await response.Content.ReadAsStringAsync();
                T? result = JsonSerializer.Deserialize<T>(json, DiscordClient.ReceiveJsonSerializerOptions);

                return result ?? throw new InvalidOperationException(
                    $"Deserialization returned null. Response: {json} (expected: {typeof(T)})"
                );
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        /// <summary>
        /// <c>True</c> when deletion was sucessful
        /// </summary>
        [DebuggerStepThrough]
        public async Task<bool> DeleteAsync(string endpoint, CallerInfos callerInfos)
        {
            SemaphoreSlim semaphoreSlim = default!;
            try
            {
                DiscordClient.Logger.LogDebug($"[DeleteAsync<T>(string endpoint)] requesting {endpoint}");
                string formattedEndpoint = FormatEndpoint(endpoint);

                semaphoreSlim = await WaitIfNeededAsync(formattedEndpoint);
                HttpResponseMessage? response = await GetDeleteRequestAsync(HttpRequestType.Delete, endpoint, callerInfos);
                if (response is null)
                    return false;

                HttpRateLimitInfo newInfo = GetRateLimitInfo(response.Headers);

                if (!string.IsNullOrEmpty(newInfo.BucketId))
                {
                    _rateLimitInfo[newInfo.BucketId] = newInfo;
                    _routeToBucketId[formattedEndpoint] = newInfo.BucketId;
                }

                return true;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        [DebuggerStepThrough]
        public async Task<TOutput?> PostAsync<TInput, TOutput>(TInput content, string endpoint, CallerInfos callerInfos)
        {
            SemaphoreSlim semaphoreSlim = default!;
            try
            {
                DiscordClient.Logger.LogDebug($"[PostAsync<T>(string endpoint)] requesting {endpoint}");
                string formattedEndpoint = FormatEndpoint(endpoint);

                semaphoreSlim = await WaitIfNeededAsync(formattedEndpoint);
                (HttpResponseMessage? response, TOutput? newObj) = await PostPatchRequestAsync<TInput, TOutput>(HttpRequestType.Post, content, endpoint, callerInfos);
                if (response is null)
                    return default;

                HttpRateLimitInfo newInfo = GetRateLimitInfo(response.Headers);

                if (!string.IsNullOrEmpty(newInfo.BucketId))
                {
                    _rateLimitInfo[newInfo.BucketId] = newInfo;
                    _routeToBucketId[formattedEndpoint] = newInfo.BucketId;
                }

                return newObj;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task<TOutput?> PatchAsync<TInput, TOutput>(TInput content, string endpoint, CallerInfos callerInfos)
        {
            SemaphoreSlim semaphoreSlim = default!;
            try
            {
                DiscordClient.Logger.LogDebug($"[PatchAsync<T>(string endpoint)] requesting {endpoint}");
                string formattedEndpoint = FormatEndpoint(endpoint);

                semaphoreSlim = await WaitIfNeededAsync(formattedEndpoint);
                (HttpResponseMessage? response, TOutput? newObj) = await PostPatchRequestAsync<TInput, TOutput>(HttpRequestType.Patch, content, endpoint, callerInfos);
                if (response is null)
                    return default;

                HttpRateLimitInfo newInfo = GetRateLimitInfo(response.Headers);

                if (!string.IsNullOrEmpty(newInfo.BucketId))
                {
                    _rateLimitInfo[newInfo.BucketId] = newInfo;
                    _routeToBucketId[formattedEndpoint] = newInfo.BucketId;
                }

                return newObj;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        #endregion

        #region Requests

        [DebuggerStepThrough]
        private async Task<HttpResponseMessage?> GetDeleteRequestAsync(HttpRequestType requestType
            , string endpoint, CallerInfos callerInfos)
        {
            HttpResponseMessage response;
            while (true)
            {
                response = requestType switch
                {
                    HttpRequestType.Get => await _httpClient.GetAsync(endpoint),
                    HttpRequestType.Delete => await _httpClient.DeleteAsync(endpoint),
                    _ => throw new InvalidEnumArgumentException("This method only allows HttpRequestType.Get or HttpRequestType.Delete"),
                };

                if (response.IsSuccessStatusCode)
                    return response;

                if (!await HandleErrorCode(response, callerInfos, endpoint, (int)response.StatusCode))
                    return null;
            }
        }

        private async Task<(HttpResponseMessage? responseMessage, TOutput? newObj)> PostPatchRequestAsync<TInput, TOutput>
            (HttpRequestType requestType, TInput content, string endpoint, CallerInfos callerInfos)
        {
            HttpResponseMessage response;
            StringContent stringContent = new(JsonSerializer.Serialize(content, DiscordClient.SendJsonSerializerOptions), Encoding.UTF8, "application/json");

            while (true)
            {
                response = requestType switch
                {
                    HttpRequestType.Patch => await _httpClient.PatchAsync(endpoint, stringContent),
                    HttpRequestType.Post => await _httpClient.PostAsync(endpoint, stringContent),
                    _ => throw new InvalidEnumArgumentException("This method only allows HttpRequestType.Patch or HttpRequestType.Post"),
                };

                using JsonDocument doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                string prettyJson = JsonSerializer.Serialize(doc.RootElement, DiscordClient.ReceiveJsonSerializerOptions);
                HttpRateLimitInfo httpRateLimitInfo = GetRateLimitInfo(response.Headers);

                DiscordClient.Logger.LogDebug($"[Request]: {FormatEndpoint(endpoint)} [{httpRateLimitInfo.BucketId}] " +
                    $"[{requestType}] [{(int)response.StatusCode}] [{response.ReasonPhrase}] [Retry-after: {httpRateLimitInfo.RetryAfter}]");
                DiscordClient.Logger.LogHttpPayload(PayloadType.Sent, requestType, await stringContent.ReadAsStringAsync());
                DiscordClient.Logger.LogError($"[Response]: {prettyJson}");
                
                if (response.IsSuccessStatusCode)
                    break;

                if (!await HandleErrorCode(response, callerInfos, endpoint, (int)response.StatusCode))
                    return (null, default);
            }

            string returnedContent = await response.Content.ReadAsStringAsync();
            TOutput? newObj = JsonSerializer.Deserialize<TOutput>(returnedContent, DiscordClient.ReceiveJsonSerializerOptions);

            if (newObj is null)
            {
                using JsonDocument jsonDoc = JsonDocument.Parse(returnedContent);
                string prettyJson = JsonSerializer.Serialize(jsonDoc.RootElement, DiscordClient.ReceiveJsonSerializerOptions);
                throw new ArgumentException($"You expected {typeof(TOutput)} but the Rest API returned something else. Returned content:\n{prettyJson}");
            }

            return (response, newObj);
        }

        #endregion

        #region HandleHttpErrors

        /// <summary>
        /// True if the error is handable. If false cancel the method
        /// </summary>
        [DebuggerStepThrough]
        private async Task<bool> HandleErrorCode(HttpResponseMessage response, CallerInfos callerInfos, string endpoint, int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    DiscordClient.Logger.LogError($"The data you sent is invalid in some form" +
                        $"[file {callerInfos.FilePath}, in {callerInfos.CallerName}(), at line: {callerInfos.LineNum}");
                    return false;
                case 403:
                    DiscordClient.Logger.LogError($"You do not have the permissions to execute that" +
                        $"[file {callerInfos.FilePath}, in {callerInfos.CallerName}(), at line: {callerInfos.LineNum}");
                    return false;
                case 404:
                    DiscordClient.Logger.LogError($"The resource you are trying to access doesn´t exist" +
                        $"[file {callerInfos.FilePath}, in {callerInfos.CallerName}(), at line: {callerInfos.LineNum}");
                    return false;
                case 429:
                    await HandleError429(response, endpoint);
                    return true;
                default:
                    throw new HttpRequestException($"HTTP {(int)response.StatusCode} {response.ReasonPhrase}");
            }
        }

        private async Task HandleError429(HttpResponseMessage response, string endpoint)
        {
            DiscordClient.Logger.LogDebug($"[RateLimit] Hit while requesting {endpoint}");

            if (CheckIfGlobal(await response.Content.ReadAsStringAsync(), out TimeSpan timeToWait))
            {
                DiscordClient.Logger.LogWarning($"[GlobalRateLimit] Blocked for {timeToWait.TotalSeconds:F2}s");
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
                DiscordClient.Logger.LogDebug($"[RateLimit] Retrying in {retryDelay.TotalSeconds:F2}s");
                await Task.Delay(retryDelay);
            }
        }

        #endregion

        #region Helper methods
        private async Task<SemaphoreSlim> WaitIfNeededAsync(string endpoint)
        {
            if (IsGlobalTimeout)
            {
                DiscordClient.Logger.LogDebug($"[GlobalRateLimit] waiting {_globalTimeout:F2}s");
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
                        DiscordClient.Logger.LogDebug($"[RateLimit] waiting {delay.TotalSeconds:F2}s for bucket {bucketId}");
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
            using JsonDocument doc = JsonDocument.Parse(content);
            JsonElement root = doc.RootElement;
            timeToWait = TimeSpan.Zero;

            if (root.TryGetProperty("global", out JsonElement global) && global.GetBoolean())
            {
                if (root.TryGetProperty("retry_after", out JsonElement retry))
                {
                    double seconds = retry.ValueKind == JsonValueKind.Number
                        ? retry.GetDouble()
                        : double.Parse(retry.GetString()!, CultureInfo.InvariantCulture);

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

        #endregion

        [GeneratedRegex(@"\d{16,19}", RegexOptions.Compiled)]
        private static partial Regex GenerateIdRegex();
    }
}
