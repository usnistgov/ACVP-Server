using System;
using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace NIST.CVP.ACVTS.Tests.Core
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
            int binStartIndex = directory.LastIndexOf("bin", StringComparison.OrdinalIgnoreCase);
            if (binStartIndex != -1)
            {
                directory = directory.Substring(0, binStartIndex);
            }

            // Clean up the directory a bit before using
            directory = Path.TrimEndingDirectorySeparator(directory);
            while (pathAdditions.StartsWith(@"..\"))
            {
                directory = Directory.GetParent(directory).FullName;
                pathAdditions = pathAdditions.Substring(3, pathAdditions.Length - 3);
            }

            // combine the directory with the pathAdditions (mostly "../../" to get back to the test files.)
            directory = Path.GetFullPath(Path.Combine(directory, pathAdditions));

            // optionally replace the "\" with a "/" if the file system needs it
            directory = directory.Replace('\\', Path.DirectorySeparatorChar);

            return directory;
        }

        /// <summary>
        /// Creates/Returns a unique path name in the form "[rootDirectory]\[prependDirectoryName][GUID]"
        /// </summary>
        /// <example>
        /// Sample input/output
        /// 
        /// <code>
        /// Console.WriteLine(Utilities.GetTestFolder(@"C:\workspace\ACVP\gen-val\trunk\temp_integrationTests\", "prePend"));
        /// </code>
        /// 
        /// > C:\workspace\ACVP\gen-val\trunk\temp_integrationTests\prePend_fa17e6e2-e77d-44f7-a6ae-95cc5883a4a2
        /// 
        /// </example>
        /// <param name="rootDirectory">The root directory in which to create the test folder</param>
        /// <param name="prependDirectoryName">Prepends the GUID with this test</param>
        /// <returns></returns>
        public static string GetTestFolder(string rootDirectory, string prependDirectoryName)
        {
            var targetFolder = Path.Combine(rootDirectory, $"{prependDirectoryName}_{Guid.NewGuid().ToString()}");
            Directory.CreateDirectory(targetFolder);

            return targetFolder;
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

        public static string BaseDir => Path.Combine(UserDocumentsDir, UNIT_TEST_FOLDER_NAME);
        public static string UserHomeDir => Environment.GetEnvironmentVariable("USERPROFILE");
        public static string UserDocumentsDir => Path.Combine(UserHomeDir, "Documents");
    }
}
