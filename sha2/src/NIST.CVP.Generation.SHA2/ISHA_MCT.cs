using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2
{
    public interface ISHA_MCT
    {
        MCTResult<AlgoArrayResponse> MCTHash(HashFunction function, BitString message, bool isSample);
    }
}
