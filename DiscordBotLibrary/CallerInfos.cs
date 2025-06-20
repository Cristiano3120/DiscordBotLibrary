using System.Runtime.CompilerServices;

namespace DiscordBotLibrary
{
    /// <summary>
    /// <c>CANT INSTANTIATE.</c> Call <see cref="Create"/> instead.
    /// </summary>
    public sealed record CallerInfos
    {
        public string CallerName { get; init; }
        public string FilePath { get; init; }
        public int LineNum { get; init; }

        private CallerInfos(string callerName, string filePath , int lineNum)
        {
            CallerName = callerName;
            FilePath = filePath;
            LineNum = lineNum;
        }

        public static CallerInfos Create([CallerMemberName] string callerName = "",[CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNum = 0)
        => new(callerName, filePath, lineNum);
    }
}
