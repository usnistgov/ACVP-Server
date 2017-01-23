using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA
{
    public class SHA2 : SHA
    {
        public override BitString HashMessage(HashFunction hashFunction, BitString message)
        {
            throw new NotImplementedException();
        }
    }
}
