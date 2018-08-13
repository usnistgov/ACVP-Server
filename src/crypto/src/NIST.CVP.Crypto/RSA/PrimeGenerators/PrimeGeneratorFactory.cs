using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;
using System;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
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
