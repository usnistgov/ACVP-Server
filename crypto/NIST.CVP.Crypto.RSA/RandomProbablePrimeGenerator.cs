using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA
{
    // B.3.3
    public class RandomProbablePrimeGenerator : PrimeGeneratorBase
    {
        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            throw new NotImplementedException();
        }
    }
}
