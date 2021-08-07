using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TheMulti0.Console.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ServiceCollection()
                .AddLogging(builder => builder.AddTheMulti0Console(options => options.IncludeThreadIds = true))
                .BuildServiceProvider()
                .GetRequiredService<ILoggerFactory>();
            
            var logger = factory.CreateLogger<Program>();
            TestLogging(logger);

            System.Console.ReadLine();
        }

        private static void TestLogging(ILogger logger)
        {
            const string message = "Test";
            
            logger.LogTrace(message);
            logger.LogDebug(message);
            logger.LogInformation(message);
            logger.LogWarning(message);
            logger.LogError(message);
            logger.LogError(new Exception(), message);
            try
            {
                throw new Exception();
            }
            catch (Exception e)
            {
                logger.LogError(e, message);
            }
            logger.LogCritical(message);
        }
    }
}