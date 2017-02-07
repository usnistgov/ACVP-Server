using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA2
{
    public interface ISHA
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message);
    }
}
