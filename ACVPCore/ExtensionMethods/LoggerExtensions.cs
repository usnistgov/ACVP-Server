using System;
using Microsoft.Extensions.Logging;

namespace ACVPCore.ExtensionMethods
{
    public static class LoggerExtensions
    {
        public static void LogError<T>(this ILogger<T> logger, Exception ex)
        {
            logger.LogError(ex, String.Empty);
        }
    }
}