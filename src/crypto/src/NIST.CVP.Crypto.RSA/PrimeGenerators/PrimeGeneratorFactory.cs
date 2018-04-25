using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class PrimeGeneratorFactory : IPrimeGeneratorFactory
    {
        private readonly HashFunction _hashFunction;
        private readonly EntropyProviderTypes _entropyType;

        public PrimeGeneratorFactory()
        {
            _hashFunction = new HashFunction { Mode = ModeValues.NONE, DigestSize = DigestSizes.NONE };
            _entropyType = EntropyProviderTypes.Random;
        }

        public PrimeGeneratorFactory(HashFunction hashFunction)
        {
            _hashFunction = hashFunction;
        }

        public PrimeGeneratorFactory(EntropyProviderTypes entropyType)
        {
            _entropyType = entropyType;
        }

        public PrimeGeneratorFactory(HashFunction hashFunction, EntropyProviderTypes entropyType)
        {
            _hashFunction = hashFunction;
            _entropyType = entropyType;
        }

        public IPrimeGeneratorBase GetPrimeGenerator(string type)
        {
            switch (type)
            {
                case "3.2":
                    return new RandomProvablePrimeGenerator(_hashFunction);

                case "3.3":
                    return new RandomProbablePrimeGenerator(_entropyType);

                case "3.4":
                    return new AllProvablePrimesWithConditionsGenerator(_hashFunction);

                case "3.5":
                    return new ProvableProbablePrimesWithConditionsGenerator(_hashFunction, _entropyType);

                case "3.6":
                    return new AllProbablePrimesWithConditionsGenerator(_entropyType);

                default:
                    return null;
            }
        }
    }
}
