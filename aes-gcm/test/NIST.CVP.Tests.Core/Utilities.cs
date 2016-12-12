using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Tests.Core
{
    public static class Utilities
    {
        /// <summary>
        /// NUnit test runner and ReSharper test runner start from different directories.  Get a consistent start directory for the runners.
        /// </summary>
        /// <example>
        /// Resharper:
        ///     C:\Users\myUser\Documents\svnWorkspace\STVM_Research\ACVP\gen-val\trunk\aes-gcm\test\NIST.CVP.Generation.AES_GCM.Tests\bin\Release\
        /// Nunit:
	    ///     C:\Users\myUser\Documents\svnWorkspace\STVM_Research\ACVP\gen-val\trunk\aes-gcm\test\NIST.CVP.Generation.AES_GCM.Tests\
        /// </example>
        /// <param name="pathAdditions">Path changes to apply after arriving at a consistent start path</param>
        public static string GetConsistentTestingStartPath(string pathAdditions)
        {
            string directory = Path.GetFullPath(@".\");
            int binStartIndex = directory.LastIndexOf(@"\bin\");

            if (binStartIndex != -1)
            {
                directory = directory.Substring(0, binStartIndex+1);
            }
            directory = Path.GetFullPath(directory + pathAdditions);

            return directory;
        }
    }
}
