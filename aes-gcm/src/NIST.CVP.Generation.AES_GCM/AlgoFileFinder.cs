using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.AES_GCM
{
    //@@@extract goodies to base class for re-use by all algos
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "AES_GCM"; } }
    }
}
