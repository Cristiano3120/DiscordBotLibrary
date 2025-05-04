using System.Text.Json.Serialization;

namespace DiscordBotLibrary.GuildScheduledEventResources.GuildScheduledEventRecurrenceRuleResources
{
    // <summary>
    /// Represents the recurrence rule for a scheduled guild event.
    /// </summary>
    public sealed record GuildScheduledEventRecurrenceRule
    {
        /// <summary>
        /// Gets the starting time of the recurrence interval.
        /// </summary>
        [JsonPropertyName("start")]
        public DateTimeOffset Start { get; init; }

        /// <summary>
        /// Gets the ending time of the recurrence interval, or null if not set.
        /// </summary>
        [JsonPropertyName("end")]
        public DateTimeOffset? End { get; init; }

        /// <summary>
        /// Gets the frequency of the recurrence rule (e.g., daily, weekly, etc.).
        /// </summary>
        [JsonPropertyName("frequency")]
        public GuildScheduledEventRecurrenceFrequency Frequency { get; init; }

        /// <summary>
        /// Gets the interval between events, defined by frequency.
        /// For example, with a frequency of WEEKLY and an interval of 2, it would be every-other week.
        /// </summary>
        [JsonPropertyName("interval")]
        public int Interval { get; init; }

        /// <summary>
        /// Gets the specific days within a week on which the event should recur, or null if not set.
        /// </summary>
        [JsonPropertyName("by_weekday")]
        public GuildScheduledEventRecurrenceWeekday[]? ByWeekday { get; init; }

        /// <summary>
        /// Gets the specific days within specific weeks (1-5) to recur on, or null if not set.
        /// </summary>
        [JsonPropertyName("by_n_weekday")]
        public GuildScheduledEventRecurrenceNWeekday[]? ByNWeekday { get; init; }

        /// <summary>
        /// Gets the specific months on which the event should recur, or null if not set.
        /// </summary>
        [JsonPropertyName("by_month")]
        public GuildScheduledEventRecurrenceMonth? ByMonth { get; init; }

        /// <summary>
        /// Gets the specific dates within a month on which the event should recur, or null if not set.
        /// </summary>
        [JsonPropertyName("by_month_day")]
        public int[]? ByMonthDay { get; init; }

        /// <summary>
        /// Gets the specific days within a year on which the event should recur (1-364), or null if not set.
        /// </summary>
        [JsonPropertyName("by_year_day")]
        public int[]? ByYearDay { get; init; }

        /// <summary>
        /// Gets the total number of times that the event is allowed to recur before stopping, or null if not set.
        /// </summary>
        [JsonPropertyName("count")]
        public int? Count { get; init; }
    }
}