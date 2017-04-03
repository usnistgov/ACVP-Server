using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA3
{
    public interface ISHA3_MCT
    {
        MCTResult<AlgoArrayResponse> MCTHash(HashFunction function, BitString message, bool isSample, int min, int max);
    }
}
