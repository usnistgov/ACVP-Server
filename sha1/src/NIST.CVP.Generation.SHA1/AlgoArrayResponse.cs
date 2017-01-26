using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1
{
    public class AlgoArrayResponse
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
    }
}
