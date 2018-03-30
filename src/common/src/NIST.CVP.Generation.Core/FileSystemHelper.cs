using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public class FileSystemHelper
    {
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
