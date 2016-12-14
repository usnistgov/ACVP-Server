using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NIST.CVP.Tests.Core
{
    public static class Utilities
    {
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
    }
}
