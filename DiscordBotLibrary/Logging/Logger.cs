using System.Diagnostics;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;

namespace DiscordBotLibrary.Logging
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

        internal void LogPayload(ConsoleColor color, string payload, PayloadType payloadType, int shardId)
        {
            Console.ForegroundColor = color;
            JsonNode jsonNode = JsonNode.Parse(payload)!;
            OpCode opCode = Enum.Parse<OpCode>(jsonNode["op"]!.ToString());

            if (opCode is OpCode.Dispatch)
            {
                FilterEventData(jsonNode, false, Event.GUILD_CREATE, Event.PRESENCE_UPDATE);
            }
            else
            {
                FilterOpCode(jsonNode, false, OpCode.PresenceUpdate);
            }

            jsonNode["op"] = opCode.ToString();

            JsonObject obj = FilterSensitiveData(jsonNode.AsObject());

            Write(color, LogLevel.Debug, $"{payloadType}[Id: {shardId}]", obj.ToString());
            Console.WriteLine("");
        }

        #region Filter

        private JsonObject FilterSensitiveData(JsonObject jsonNode)
        {
            if (jsonNode["d"] is not JsonObject dObj)
                return jsonNode;

            foreach (string key in _sensitiveKeys)
            {
                dObj.Remove(key);
            }

            return jsonNode;
        }

        /// <summary>
        /// Clears the event data from the JSON node based on the provided events.
        /// This leads to a cleaner log output, as only the at the moment relevant event data is logged.
        /// <para>
        /// <paramref name="onlyLogThose"/> indicates whether to log only the specified events and filter the rest out
        /// or to filter them out and only log the rest.
        /// </para>
        /// </summary>
        /// <param name="jsonNode"></param>
        /// <param name="onlyLogThose"></param>
        /// <param name="events"></param>
        private void FilterEventData(JsonNode jsonNode, bool onlyLogThoseEvents, params Event[] events)
        {
            if (!Enum.TryParse(jsonNode["t"]!.ToString()!, out Event dispatchEvent))
            {
                LogError(new Exception($"Event {jsonNode["t"]} not found in the Event enum"));
                return;
            }

            if (onlyLogThoseEvents && !events.Contains(dispatchEvent) || !onlyLogThoseEvents && events.Contains(dispatchEvent))
            {
                jsonNode["d"] = "";
            }
        }

        private static void FilterOpCode(JsonNode jsonNode, bool onlyLogThoseEvents, params OpCode[] opCodes)
        {
            OpCode opCode = Enum.Parse<OpCode>(jsonNode["op"]!.ToString()!);
            if (onlyLogThoseEvents && !opCodes.Contains(opCode) || !onlyLogThoseEvents && opCodes.Contains(opCode))
            {
                jsonNode["d"] = "";
            }
        }

        #endregion

        #region ExternalAddMethods

        /// <summary>
        /// Adds a key to the list of sensitive keys. The key removed in the log/console
        /// This is useful for removing sensitive data like tokens or passwords from the log/console.
        /// By default, the key "token" is added to the list of sensitive keys.
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
