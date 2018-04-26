using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public class PrimeGeneratorFactory : IPrimeGeneratorFactory
    {
        public IPrimeGenerator GetPrimeGenerator(PrimeGenModes primeGen, ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest)
        {
            switch (primeGen)
            {
                case PrimeGenModes.B32:
                    return new RandomProvablePrimeGenerator(sha);

                case PrimeGenModes.B33:
                    return new RandomProbablePrimeGenerator(entropyProvider, primeTest);

                case PrimeGenModes.B34:
                    return new AllProvablePrimesWithConditionsGenerator(sha);

                case PrimeGenModes.B35:
                    return new ProvableProbablePrimesWithConditionsGenerator(sha, entropyProvider);

                case PrimeGenModes.B36:
                    return new AllProbablePrimesWithConditionsGenerator(entropyProvider, primeTest);

                default:
                    throw new ArgumentException("Invalid prime gen mode");
            }
        }
    }
}
