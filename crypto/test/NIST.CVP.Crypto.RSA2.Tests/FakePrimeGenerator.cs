using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using System.Numerics;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;

namespace NIST.CVP.Crypto.RSA2.Tests
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
