using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "TDES"; } }
        public override string FilePrefix { get { return "TCBC"; } }
    }
}
