using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Math;
using System;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.Tests
{
    public class FakePrimeGenerator : PrimeGeneratorBase
    {
        public FakePrimeGenerator(ISha sha) : base(sha) { }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            throw new NotImplementedException();
        }
    }
}
