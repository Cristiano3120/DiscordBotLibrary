namespace DiscordBotLibrary.RestApiLimiterResources
{
    public sealed record HttpRateLimitInfo
    {
        public string? BucketId { get; set; }
        public int? Limit { get; set; }
        public int? Remaining { get; set; }
        public DateTimeOffset? ResetAt { get; set; }
        public TimeSpan? RetryAfter { get; set; }
    }
}
