using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;
using System;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class PrimeGeneratorFactory : IPrimeGeneratorFactory
    {
        public IFips186_5PrimeGenerator GetFips186_5PrimeGenerator(PrimeGenModes primeGen, ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest)
        {
            return GetPrimeGenerator(primeGen, sha, entropyProvider, primeTest) as IFips186_5PrimeGenerator;
        }

        public IFips186_4PrimeGenerator GetFips186_4PrimeGenerator(PrimeGenModes primeGen, ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest)
        {
            return GetPrimeGenerator(primeGen, sha, entropyProvider, primeTest) as IFips186_4PrimeGenerator;
        }

        public IFips186_2PrimeGenerator GetFips186_2PrimeGenerator(IEntropyProvider entropyProvider, PrimeTestModes primeTest)
        {
            return new RandomProbablePrimeGenerator(entropyProvider, primeTest);
        }

        private static IPrimeGenerator GetPrimeGenerator(PrimeGenModes primeGen, ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest)
        {
            switch (primeGen)
            {
                case PrimeGenModes.RandomProvablePrimes:
                    return new RandomProvablePrimeGenerator(sha);

                case PrimeGenModes.RandomProbablePrimes:
                    return new RandomProbablePrimeGenerator(entropyProvider, primeTest);

                case PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes:
                    return new AllProvablePrimesWithConditionsGenerator(sha);

                case PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes:
                    return new ProvableProbablePrimesWithConditionsGenerator(sha, entropyProvider, primeTest);

                case PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes:
                    return new AllProbablePrimesWithConditionsGenerator(entropyProvider, primeTest);

                default:
                    throw new ArgumentException("Invalid prime gen mode");
            }
        }
    }
}
