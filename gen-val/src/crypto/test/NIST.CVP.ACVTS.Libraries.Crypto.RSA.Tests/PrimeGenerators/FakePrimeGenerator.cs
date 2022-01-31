using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Tests.PrimeGenerators
{
    public class FakePrimeGenerator : IFips186_5PrimeGenerator, IFips186_4PrimeGenerator, IFips186_2PrimeGenerator
    {
        public PrimeGeneratorResult GeneratePrimesFips186_5(PrimeGeneratorParameters param)
        {
            throw new NotImplementedException();
        }

        public PrimeGeneratorResult GeneratePrimesFips186_4(PrimeGeneratorParameters param)
        {
            throw new NotImplementedException();
        }

        public PrimeGeneratorResult GeneratePrimesFips186_2(PrimeGeneratorParameters param)
        {
            throw new NotImplementedException();
        }
    }
}
