using System.Text.Json.Serialization;

namespace DiscordBotLibrary.GuildScheduledEventResources.GuildScheduledEventRecurrenceRuleResources
{
    /// <summary>
    /// Represents a specific day within a week on which a scheduled event should recur.
    /// </summary>
    public readonly struct GuildScheduledEventRecurrenceNWeekday
    {
        /// <summary>
        /// The week to reoccur on. 1 - 5
        /// </summary>
        [JsonPropertyName("n")]
        public int N { get; init; }

        /// <summary>
        /// The day within the week to reoccur on
        /// </summary>
        [JsonPropertyName("day")]
        public GuildScheduledEventRecurrenceWeekday Day { get; init; }
    }
}