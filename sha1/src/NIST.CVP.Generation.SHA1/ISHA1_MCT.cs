using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1
{
    public interface ISHA1_MCT
    {
        MCTResult MCTHash(BitString seed);
    }
}
