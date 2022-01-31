using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    public class LoggingHelper
    {
        public static void ConfigureLogging(string requestFile, string algoTag, LogLevel minLevel = null)
        {
            if (minLevel == null)
            {
                minLevel = LogLevel.Info;
            }

            var config = new LoggingConfiguration();


            AddTarget(GetConsoleTarget(), config, minLevel);
            var fileTarget = GetFileTarget(requestFile, algoTag);
            if (fileTarget != null)
            {
                AddTarget(fileTarget, config, minLevel);
            }
            LogManager.Configuration = config;
        }

        private static void AddTarget(Target target, LoggingConfiguration config, LogLevel minLevel)
        {
            config.AddTarget(target);
            config.LoggingRules.Add(new LoggingRule("*", minLevel, target));
            Logger.Debug($"Added {target.Name} Logging");
        }

        private static Target GetFileTarget(string requestFile, string algoTag)
        {
            if (string.IsNullOrEmpty(requestFile))
            {
                return null;
            }
            var fileTarget = new FileTarget("File");

            string baseDir = Path.GetDirectoryName(requestFile);
            fileTarget.FileName = Path.Combine(baseDir, $"{algoTag}.log");
            fileTarget.Layout = "${longdate} ${level} ${logger} ${message}";
            fileTarget.DeleteOldFileOnStartup = true;
            return fileTarget;
        }

        private static Target GetConsoleTarget()
        {
            var consoleTarget = new ConsoleTarget("Console");
            consoleTarget.Layout = "${level} ${logger} ${message}";
            return consoleTarget;

        }

        private static Logger Logger
        {
            get { return LogManager.GetLogger("LoggingHelper"); }
        }
    }
}
