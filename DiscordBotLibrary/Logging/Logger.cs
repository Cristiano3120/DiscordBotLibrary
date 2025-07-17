using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json.Nodes;
using DiscordBotLibrary.MessageResources;

namespace DiscordBotLibrary.Logging
{
    public sealed class Logger
    {
        private string[] _sensitiveKeys = ["token"];
        private readonly LoggerConfig _loggerConfig;
        private readonly string _pathToLogFile;

        public Logger(LoggerConfig loggerConfig)
        {
            _loggerConfig = loggerConfig;
            _pathToLogFile = MaintainLoggingSystem();
        }

        public void Log(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Info:
                    Write(ConsoleColor.White, LogLevel.Info, message, null);
                    break;
                case LogLevel.Warning:
                    Write(ConsoleColor.Yellow, LogLevel.Warning, message, null);
                    break;
                case LogLevel.Debug:
                    Write(ConsoleColor.DarkGray, LogLevel.Debug, message, null);
                    break;
            }
        }

        public void LogError<T>(T exception, bool logOnlyToFile = false) where T : Exception
        {
            if (exception is UnobservedTaskExceptionEventArgs unobservedEx)
            {
                foreach (Exception innerEx in unobservedEx.Exception.Flatten().InnerExceptions)
                {
                    LogError(innerEx);
                }
                return;
            }

            Exception ex = exception;

            StackTrace stackTrace = new(ex, true);
            StackFrame? stackFrame = stackTrace
                .GetFrames()?.FirstOrDefault(x =>
                {
                    MethodBase? method = x.GetMethod();
                    string? ns = method?.DeclaringType?.Namespace;

                    return x.GetFileName() is not null
                        && method is not null
                        && ns is not null
                        && ns.Contains("DiscordBotLibrary", StringComparison.Ordinal);
                });

            if (stackFrame is not null)
            {
                string methodName = stackFrame?.GetMethod()?.Name + "()";
                string filename = stackFrame?.GetFileName() ?? "missing filename";
                int lineNum = stackFrame?.GetFileLineNumber() ?? 0;
                int columnNum = stackFrame?.GetFileColumnNumber() ?? 0;

                int index = filename.LastIndexOf('\\') + 1;
                filename = filename[index..];

                string errorInfos = $"ERROR in file {filename}, in {methodName}, at line: {lineNum}, at column: {columnNum}";
                Write(ConsoleColor.Red, LogLevel.Error, errorInfos, null, logOnlyToFile);
            }

            Write(ConsoleColor.Red, LogLevel.Error, $"ERROR: {ex.Message}", null, logOnlyToFile);
            Write(ConsoleColor.Red, LogLevel.Error, $"{ex}\n", null, logOnlyToFile);

            if (ex.InnerException is not null)
                LogError(ex.InnerException);
        }

        /// <summary>
        /// [ERROR]: Will be infront of every log message.
        /// </summary>
        public void LogError(string exception, CallerInfos callerInfos)
        {
            string errorInfos = $"ERROR in file {callerInfos.FilePath}, in {callerInfos.CallerName}(...)";
            Write(ConsoleColor.Red, LogLevel.Error, errorInfos, null);
            Write(ConsoleColor.Red, LogLevel.Error, $"{exception}\n", null);
        }

        internal void LogWssPayload(PayloadType payloadType, string payload, int shardId)
        {
            ConsoleColor color = payloadType switch
            {
                PayloadType.Received => ConsoleColor.DarkGreen,
                PayloadType.Sent => ConsoleColor.DarkCyan,
                _ => throw new NotImplementedException($"This PayloadType is not implemented yet. " +
                    $"At: {CallerInfos.Create().CallerName}")
            };

            JsonNode jsonNode = JsonNode.Parse(payload)!;
            OpCode opCode = Enum.Parse<OpCode>(jsonNode["op"]!.ToString(), true);

            if (opCode is OpCode.Dispatch)
            {
                FilterEventData(jsonNode, LogRules.OnlyTheOthers, Event.GUILD_CREATE);
            }
            else
            {
                FilterOpCode(jsonNode, LogRules.None, receivedOpCode: opCode);
            }

            jsonNode["op"] = opCode.ToString();
            JsonObject obj = FilterSensitiveData(jsonNode.AsObject());

            Write(color, LogLevel.Debug, $"{obj}\n", $"{payloadType}[Id: {shardId}]");
        }

        /// <summary>
        /// Formats the HTTP payload and writes it to the console and the log file.
        /// </summary>
        internal void LogHttpPayload(PayloadType payloadType, HttpRequestType requestType, string content)
        {
            string prettyJson = JToken.Parse(content).ToString(Formatting.Indented);
            ConsoleColor color = payloadType switch
            {
                PayloadType.Received => ConsoleColor.DarkGreen,
                PayloadType.Sent => ConsoleColor.DarkCyan,
                _ => throw new NotImplementedException($"This PayloadType is not implemented yet. " +
                    $"At: {CallerInfos.Create().CallerName}")
            };

            Write(color, LogLevel.Debug, prettyJson, $"{payloadType}({requestType})");
        }

        public void CustomLog(ConsoleColor color, LogLevel level, string message, string? tag = null, bool logOnlyToFile = false)
            => Write(color, level, message, tag, logOnlyToFile);
        
        private string MaintainLoggingSystem()
        {
            string pathToLoggingDic = Helper.GetDynamicPath(_loggerConfig.PathToLoggingFolder);
            if (!Directory.Exists(pathToLoggingDic))
            {
                Directory.CreateDirectory(pathToLoggingDic);
            }
            else
            {
                string[] files = Directory.GetFiles(pathToLoggingDic, "*.md");

                if (files.Length >= _loggerConfig.MaxAmmountOfLoggingFiles)
                {
                    files = [.. files.OrderBy(File.GetCreationTime)];
                    // +1 to make room for a new File
                    int filesToRemove = files.Length - _loggerConfig.MaxAmmountOfLoggingFiles + 1;

                    for (int i = 0; i < filesToRemove; i++)
                    {
                        File.Delete(files[i]);
                    }
                }
            }

            string timestamp = DateTime.Now.ToString("dd-MM-yyyy/HH-mm-ss");
            string pathToNewFile = Helper.GetDynamicPath($"{pathToLoggingDic}{timestamp}.md");
            File.Create(pathToNewFile).Close();

            return pathToNewFile;
        }

        /// <summary>
        /// <paramref name="tag"/> is gonna be level.ToString()
        /// </summary>
        private void Write(ConsoleColor color, LogLevel level
            , string message, string? tag, bool logOnlyToFile = false)
        {
            if (_loggerConfig.LogLevel < level)
                return;

            if (string.IsNullOrEmpty(tag))
                tag = level.ToString();

            string log = $"[{DateTime.Now}] [{tag}]: {message}";
            if (!logOnlyToFile)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(log);
                Console.ResetColor();
            }

            using (StreamWriter streamWriter = new(_pathToLogFile, true))
            {
                streamWriter.WriteLine(log);
            }
        }

        #region Filter
        private JsonObject FilterSensitiveData(JsonObject jsonObj)
        {
            if (jsonObj["d"] is not JsonObject dObj)
                return jsonObj;

            foreach (string key in _sensitiveKeys)
            {
                dObj.Remove(key);
            }

            return jsonObj;
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
        private void FilterEventData(JsonNode jsonNode, LogRules logRules, params Event[] events)
        {
            if (logRules is LogRules.None)
            {
                jsonNode["d"] = "";
                return;
            }

            if (!Enum.TryParse(jsonNode["t"]!.ToString()!, true, out Event dispatchEvent))
            {
                LogError(new Exception($"Event {jsonNode["t"]} not found in the Event enum"));
                return;
            }

            if (logRules is LogRules.OnlyThose && !events.Contains(dispatchEvent)
                || logRules is LogRules.OnlyTheOthers && events.Contains(dispatchEvent))
            {
                jsonNode["d"] = "";
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
        private void FilterOpCode(JsonNode jsonNode, LogRules logRules, OpCode receivedOpCode, params OpCode[] opCodes)
        {
            if (logRules is LogRules.None || logRules is LogRules.OnlyThose && !opCodes.Contains(receivedOpCode)
                || logRules is LogRules.OnlyTheOthers && opCodes.Contains(receivedOpCode))
            {
                jsonNode["d"] = "";
            }
        }

        #endregion

        /// <summary>
        /// Adds a key to the list of sensitive keys. The key removed in the log/console
        /// This is useful for removing sensitive data like tokens or passwords from the log/console.
        /// By default, the key "token" is added to the list of sensitive keys.
        /// </summary>
        /// <param name="key"></param>
        public void AddSensitiveKeys(params string[] keys)
        {
            if (keys == null || keys.Length == 0)
                return;

            int originalLength = _sensitiveKeys.Length;
            Array.Resize(ref _sensitiveKeys, originalLength + keys.Length);
            Array.Copy(keys, 0, _sensitiveKeys, originalLength, keys.Length);
        }
    }
}
