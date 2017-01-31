using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA
{
    public struct HashFunction
    {
        public ModeValues Mode { get; set; }
        public DigestSizes DigestSize { get; set; }
    }
}
