using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public class PrimeGeneratorFactory : IPrimeGeneratorFactory
    {
        private readonly IEntropyProviderFactory _factory;

        public PrimeGeneratorFactory(IEntropyProviderFactory entropyProviderFactory)
        {
            _factory = entropyProviderFactory;
        }

        public IPrimeGenerator GetPrimeGenerator(PrimeGenModes primeGen, ISha sha, EntropyProviderTypes entropyType, PrimeTestModes primeTest)
        {
            var entropyProvider = _factory.GetEntropyProvider(entropyType);

            switch (primeGen)
            {
                case PrimeGenModes.B32:
                    return new RandomProvablePrimeGenerator(sha);

                case PrimeGenModes.B33:
                    return new RandomProbablePrimeGenerator(entropyProvider);

                case PrimeGenModes.B34:
                    return null;

                case PrimeGenModes.B35:
                    return null;

                case PrimeGenModes.B36:
                    return null;

                default:
                    return null;
            }
        }
    }
}
