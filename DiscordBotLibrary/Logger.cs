using System.Diagnostics;
using System.Text.Json.Nodes;

namespace DiscordBotLibrary
{
    public sealed class Logger
    {
        private readonly HashSet<string> _sensitiveKeys = ["token"];

        private const byte _maxAmmountLoggingFiles = 10;
        private readonly string _pathToLogFile;
        private readonly LogLevel _logLevel;

        public Logger(LogLevel logLevel)
        {
            _pathToLogFile = MaintainLoggingSystem(_maxAmmountLoggingFiles);
            _logLevel = logLevel;
        }

        private static string MaintainLoggingSystem(int maxAmmountLoggingFiles)
        {
            string pathToLoggingDic = GetDynamicPath(@"Logging/");
            if (!Directory.Exists(pathToLoggingDic))
            {
                Directory.CreateDirectory(pathToLoggingDic);
            }
            else
            {
                string[] files = Directory.GetFiles(pathToLoggingDic, "*.md");

                if (files.Length >= maxAmmountLoggingFiles)
                {
                    files = [.. files.OrderBy(File.GetCreationTime)];
                    // +1 to make room for a new File
                    int filesToRemove = files.Length - maxAmmountLoggingFiles + 1;

                    for (int i = 0; i < filesToRemove; i++)
                    {
                        File.Delete(files[i]);
                    }
                }
            }

            string timestamp = DateTime.Now.ToString("dd-MM-yyyy/HH-mm-ss");
            string pathToNewFile = GetDynamicPath($"Logging/{timestamp}.md");
            File.Create(pathToNewFile).Close();

            return pathToNewFile;
        }

        private void Write(ConsoleColor color, LogLevel level, string tag, string message)
        {
            Console.ForegroundColor = color;
            if (_logLevel >= level)
            {
                string log = $"[{DateTime.Now}] [{tag}]: {message}";
                Console.WriteLine(log);
                using (StreamWriter streamWriter = new(_pathToLogFile, true))
                {
                    streamWriter.WriteLine(log);
                }
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
                string methodName = stackFrame?.GetMethod()?.Name + "()";
                string filename = stackFrame?.GetFileName() ?? "missing filename";
                int lineNum = stackFrame?.GetFileLineNumber() ?? 0;
                int columnNum = stackFrame?.GetFileColumnNumber() ?? 0;

                int index = filename.LastIndexOf('\\') + 1;
                filename = filename[index..];

                string errorInfos = $"ERROR in file {filename}, in {methodName}, at line: {lineNum}, at column: {columnNum}";
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

        public void LogPayload(ConsoleColor color, string payload, string prefix)
        {
            Console.ForegroundColor = color;

            JsonNode jsonNode = JsonNode.Parse(payload)!;

            jsonNode["op"] = Enum.Parse<OpCode>(jsonNode["op"]!.ToString()).ToString();
            FilterSensitiveData(jsonNode);
            
            Write(color, LogLevel.Debug, prefix, jsonNode.ToString());
            Console.WriteLine("");
        }

        #region Filter

        private void FilterSensitiveData(JsonNode jsonNode)
        {
            foreach (string key in _sensitiveKeys)
            {
                if (jsonNode["d"]?[key] is JsonNode tokenNode)
                    tokenNode.ReplaceWith("**********");
            }
        }

        #endregion

        #region ExternalAddMethods

        /// <summary>
        /// Adds a key to the list of sensitive keys. The key will be replaced with "**********" in the log/console.
        /// For example if one of ur json payloads contains a key "token" and u want to log it, u can add the key to the list 
        /// of sensitive keys and it will be replaced with "**********".
        /// In the case of the key "token" this is done by default.
        /// </summary>
        /// <param name="key"></param>
        public void AddSensitiveKey(string key)
            => _sensitiveKeys.Add(key);

        #endregion

        public static string GetDynamicPath(string relativePath)
        {
            string projectBasePath = AppContext.BaseDirectory;
            int binIndex = projectBasePath.IndexOf($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal);

            if (binIndex == -1)
            {
                throw new Exception("Could not determine project base path!");
            }

            projectBasePath = projectBasePath[..binIndex];
            return Path.Combine(projectBasePath, relativePath);
        }
    }
}
