using System;
using Microsoft.Extensions.Logging;

namespace TheMulti0.Console
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddTheMulti0Console(this ILoggingBuilder builder)
        {
            return builder.AddTheMulti0Console(options => { });
        }
        
        public static ILoggingBuilder AddTheMulti0Console(this ILoggingBuilder builder, Action<TheMulti0ConsoleFormatterOptions> configure)
        {
            return builder
                .AddConsole(options => options.FormatterName = TheMulti0LoggingConstants.Name)
                .AddConsoleFormatter<TheMulti0ConsoleFormatter, TheMulti0ConsoleFormatterOptions>(configure);
        }
    }
}