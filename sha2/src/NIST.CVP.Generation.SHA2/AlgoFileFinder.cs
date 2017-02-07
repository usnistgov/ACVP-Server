using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "SHA"; } }
        public override string FilePrefix { get { return "SHA"; } }
    }
}
