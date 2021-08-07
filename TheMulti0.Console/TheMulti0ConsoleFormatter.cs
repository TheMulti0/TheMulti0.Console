using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace TheMulti0.Console
{
    internal class TheMulti0ConsoleFormatter : ConsoleFormatter
    {
        private readonly string _timestampFormat;
        private readonly bool _useUtcTimestamp;
        private readonly bool _includeThreadIds;

        public TheMulti0ConsoleFormatter(IConfigureOptions<TheMulti0ConsoleFormatterOptions> configure) 
            : base(TheMulti0LoggingConstants.Name)
        {
            var options = new TheMulti0ConsoleFormatterOptions();
            configure.Configure(options);
            
            _timestampFormat = string.IsNullOrEmpty(options.TimestampFormat) 
                ? "yyyy-MM-dd HH:mm:ss zzz" 
                : options.TimestampFormat;
            _useUtcTimestamp = options.UseUtcTimestamp;
            _includeThreadIds = options.IncludeThreadIds;
        }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider scopeProvider, TextWriter textWriter)
        {
            IEnumerable<string> prefixes = GetPrefixes(logEntry);

            foreach (string prefix in prefixes)
            {
                textWriter.Write($"[{prefix}] ");
            }

            string formattedMessage = Format(logEntry.State, logEntry.Exception);
            textWriter.Write(formattedMessage);

            textWriter.WriteLine();
        }

        private IEnumerable<string> GetPrefixes<TState>(LogEntry<TState> logEntry)
        {
            DateTime dateTime = _useUtcTimestamp
                ? DateTime.UtcNow
                : DateTime.Now;

            yield return dateTime.ToString(_timestampFormat); // formattedDate
            yield return logEntry.LogLevel.ToString(); // formattedLevel

            if (_includeThreadIds)
            {
                yield return $"Thread {Thread.CurrentThread.ManagedThreadId}";
            }
            
            yield return logEntry.Category; // categoryName
        }
        
        private static string Format<TState>(TState state, Exception exception)
        {
            if (exception == null)
            {
                return state.ToString();
            }
            
            string stackTrace = exception.StackTrace == null 
                ? string.Empty 
                : $"\n{exception.StackTrace}";
                
            return $"{state}\nUnhandled exception. {exception.GetType().Name}{stackTrace}";
        }
    }
}