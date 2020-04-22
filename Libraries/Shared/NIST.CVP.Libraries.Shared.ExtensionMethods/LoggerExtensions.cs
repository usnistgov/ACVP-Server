using System;
using Microsoft.Extensions.Logging;

namespace NIST.CVP.Libraries.Shared.ExtensionMethods
{
    public static class LoggerExtensions
    {
        public static void LogWarning<T>(this ILogger<T> logger, Exception ex)
        {
            logger.LogWarning(ex, String.Empty);
        }
        
        public static void LogError<T>(this ILogger<T> logger, Exception ex)
        {
            logger.LogError(ex, String.Empty);
        }
    }
}