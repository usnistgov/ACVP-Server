using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public interface IPrimeGenerator
    {
        PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed);
    }
}
