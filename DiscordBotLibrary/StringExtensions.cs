namespace DiscordBotLibrary
{
    internal static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            return !string.IsNullOrEmpty(str)
                ? char.ToLowerInvariant(str[0]) + str[1..]
                : str;
        }
    }
}
