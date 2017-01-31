using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace NIST.CVP.Tests.Core
{
    public static class Utilities
    {
        private const string UNIT_TEST_FOLDER_NAME = "UnitTests/ACAVP";

        /// <summary>
        /// NUnit test runner and ReSharper test runner start from different directories.  Get a consistent start directory for the runners.
        /// </summary>
        /// <example>
        /// ReSharper:
        ///     C:\Users\myUser\Documents\project\test\NameSpace.Tests\bin\Release\
        /// Nunit:
        ///     C:\Users\myUser\Documents\project\test\NameSpace.Tests\
        /// ReSharper - dotCover
        ///     ?? Shadow copy directory is this ./bin/Release or ./ ???
        /// TeamCity - dotCover
        ///     ?? - TODO
        /// </example>
        /// <param name="typeToGetAssemblyInfoFrom">The object to get the assembly information from.</param>
        /// <param name="pathAdditions">Path changes to apply after arriving at a consistent start path</param>
        public static string GetConsistentTestingStartPath(Type typeToGetAssemblyInfoFrom, string pathAdditions)
        {
            // drop that file:\\ stuff
            var directory = new Uri(
                // Get the directory name from the executing assembly
                Path.GetDirectoryName(
                    typeToGetAssemblyInfoFrom.GetTypeInfo().Assembly.Location)
                ).LocalPath;

            // drop the \bin\* from the directory
            int binStartIndex = directory.LastIndexOf(@"\bin\", StringComparison.OrdinalIgnoreCase);
            if (binStartIndex != -1)
            {
                directory = directory.Substring(0, binStartIndex + 1);
            }

            // combine the directory with the pathAdditions (mostly "../../" to get back to the test files.)
            directory = Path.GetFullPath(directory + pathAdditions);

            return directory;
        }


        public static void ConfigureLogging(string testTag, bool includeFile = true)
        {
            var config = new LoggingConfiguration();

            var debugTarget = new DebugTarget($"{testTag}");
            debugTarget.Layout = "${message}";
            config.AddTarget(debugTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debugTarget));

            var consoleTarget = new ConsoleTarget($"{testTag}_C");
            consoleTarget.Layout = "${message}";
            config.AddTarget(consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

            if (includeFile)
            {
                var fileTarget = new FileTarget();
                config.AddTarget("file", fileTarget);
                fileTarget.FileName = Path.Combine(BaseDir, $"test_{testTag}.log");
                fileTarget.Layout = "${logger}:  ${message}";
                fileTarget.DeleteOldFileOnStartup = true;
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));
            }
         

            LogManager.Configuration = config;
        }

        public static string BaseDir
        {
            get { return Path.Combine(UserDocumentsDir, UNIT_TEST_FOLDER_NAME); }
        }

        public static string UserHomeDir
        {
            get { return Environment.GetEnvironmentVariable("USERPROFILE"); }
        }
        public static string UserDocumentsDir
        {
            get { return Path.Combine(UserHomeDir, "Documents"); }
        }
    }
}
