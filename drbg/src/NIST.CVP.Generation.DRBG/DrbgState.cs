using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG
{
    public class DrbgState
    {
        public BitString LastEntropy { get; set; }
        public BitString LastNonce { get; set; }
    }
}
