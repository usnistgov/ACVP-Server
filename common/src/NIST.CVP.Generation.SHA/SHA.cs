using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA
{
    public abstract class SHA
    {
        public abstract BitString HashMessage(HashFunction hashFunction, BitString message);
    }
}
