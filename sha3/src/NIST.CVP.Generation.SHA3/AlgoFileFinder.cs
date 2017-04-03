using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "SHA3"; } }
        public override string FilePrefix { get { return "SHA3"; } }
    }
}
