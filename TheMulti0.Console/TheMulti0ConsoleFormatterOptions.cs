using Microsoft.Extensions.Logging.Console;

namespace TheMulti0.Console
{
    public class TheMulti0ConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public bool IncludeThreadIds { get; set; }
    }
}