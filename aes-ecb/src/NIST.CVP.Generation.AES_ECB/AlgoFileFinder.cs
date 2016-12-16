using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.AES_ECB
{
    
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "AES"; } }
        public override string FilePrefix { get { return "ECB"; } }
    }
}
