using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA2
{
    public class AlgoArrayResponse
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
    }
}
