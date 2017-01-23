using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "SHA1"; } }
    }
}
