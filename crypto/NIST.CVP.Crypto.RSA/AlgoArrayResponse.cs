using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA
{
    public class AlgoArrayResponse
    {
        public BitString E { get; set; }
        public BitString P { get; set; }
        public BitString Q { get; set; }
        public bool FailureTest { get; set; }
    }
}
