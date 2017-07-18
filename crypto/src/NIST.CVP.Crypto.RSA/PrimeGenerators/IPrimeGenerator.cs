using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public interface IPrimeGenerator
    {
        PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed);
    }
}
