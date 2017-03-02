using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA3
{
    public interface ISHA3
    {
        HashResult HashMessage(BitString message);
    }
}
