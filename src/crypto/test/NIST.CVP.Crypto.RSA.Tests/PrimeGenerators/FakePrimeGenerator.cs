using NIST.CVP.Crypto.RSA.PrimeGenerators;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;

namespace NIST.CVP.Crypto.RSA.Tests.PrimeGenerators
{
    public class FakePrimeGenerator : PrimeGeneratorBase
    {
        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            throw new NotImplementedException();
        }
    }
}
