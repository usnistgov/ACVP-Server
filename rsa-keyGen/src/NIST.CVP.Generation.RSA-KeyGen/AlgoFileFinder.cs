using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "RSA-KeyGen"; } }
        public override string FilePrefix { get { return "RSA-KeyGen"; } }
    }
}
