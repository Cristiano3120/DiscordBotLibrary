using System.Diagnostics;
using System.Text.Json.Nodes;

namespace DiscordBotLibrary
{
    internal sealed class Logger
    {
        private readonly LogLevel _logLevel;

        public Logger(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        private void Write(ConsoleColor color, LogLevel level, string tag, string message)
        {
            Console.ForegroundColor = color;
            if (_logLevel >= level)
            {
                Console.WriteLine($"[{DateTime.Now}] [{tag}]: {message}");
            }
            Console.ResetColor();
        }

        public void LogInfo(string message)
        {
            Write(ConsoleColor.White, LogLevel.Info, nameof(LogLevel.Info), message);
        }

        public void LogWarning(string message)
        {
            Write(ConsoleColor.Yellow, LogLevel.Warning, nameof(LogLevel.Warning), message);
        }

        /// <summary>
        /// Logs the error in red in the console with the error message and the file, method, line and column where the error occured
        /// </summary>
        /// <typeparam name="T">Has to be of type <c>EXCEPTION</c>, <c>UnobservedTaskExceptionEventArgs</c>, <c>NpgsqlException</c> <c>string</c> </typeparam>
        /// <exception cref="ArgumentException"></exception>
        public void LogError<T>(T exception)
        {
            string tag = nameof(LogLevel.Error);
            if (exception is string str)
            {
                Write(ConsoleColor.Red, LogLevel.Error, tag, str);
                return;
            }

            if (exception is UnobservedTaskExceptionEventArgs unobservedEx)
            {
                foreach (Exception innerEx in unobservedEx.Exception.Flatten().InnerExceptions)
                {
                    LogError(innerEx);
                }
                return;
            }

            Exception ex = exception as Exception
                ?? throw new ArgumentException($"Type {typeof(T).Name} must be of type EXCEPTION, UnobservedTaskExceptionEventArgs, NpgsqlExceptin or string.");

            StackTrace stackTrace = new(ex, true);
            StackFrame? stackFrame = null;
            foreach (StackFrame item in stackTrace.GetFrames())
            {
                //Looking for the frame contains the infos about the error
                if (item.GetMethod()?.Name is not null && item.GetFileName() is not null)
                {
                    stackFrame = item;
                    break;
                }
            }

            if (stackFrame is not null)
            {
                var methodName = stackFrame?.GetMethod()?.Name + "()";
                var filename = stackFrame?.GetFileName() ?? "missing filename";
                var lineNum = stackFrame?.GetFileLineNumber();
                var columnNum = stackFrame?.GetFileColumnNumber();

                var index = filename.LastIndexOf('\\') + 1;
                filename = filename[index..];

                var errorInfos = $"ERROR in file {filename}, in {methodName}, at line: {lineNum}, at column: {columnNum}";
                Write(ConsoleColor.Red, LogLevel.Error, tag, errorInfos);
            }

            Write(ConsoleColor.Red, LogLevel.Error, tag, $"ERROR: {ex.Message}");

            if (ex.InnerException is not null)
                LogError(ex.InnerException);
        }

        public void LogDebug(string message)
        {
            Write(ConsoleColor.Cyan, LogLevel.Debug, nameof(LogLevel.Debug), message);
        }

        public static void LogPayload(ConsoleColor color, string payload, string prefix)
        {
            Console.ForegroundColor = color;

            JsonNode jsonNode = JsonNode.Parse(payload)!;

            string opCode = nameof(OpCode).ToCamelCase();
            jsonNode[opCode] = Enum.Parse<OpCode>(jsonNode["op"]!.ToString()).ToString();

            Console.WriteLine($"[{DateTime.Now:HH: dd: ss}]: {prefix} {jsonNode}");
            Console.WriteLine("");
        }
    }
}
