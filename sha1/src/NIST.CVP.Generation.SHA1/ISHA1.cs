using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NIST.CVP.Generation.SHA;

namespace NIST.CVP.Generation.SHA1
{
    public interface ISHA1
    {
        HashResult HashMessage(BitString digest);
    }
}
