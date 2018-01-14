using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.SHA2;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        RandomProbablePrimeGenerator _primeGen;
        AllProvablePrimesWithConditionsGenerator _smallPrimeGen;

        // For Moq
        public TestGroupGeneratorFactory(RandomProbablePrimeGenerator primeGen, AllProvablePrimesWithConditionsGenerator smallPrimeGen)
        {
            _primeGen = primeGen;
            _smallPrimeGen = smallPrimeGen;
        }

        public TestGroupGeneratorFactory()
        {
            _primeGen = new RandomProbablePrimeGenerator(EntropyProviderTypes.Random);
            _smallPrimeGen = new AllProvablePrimesWithConditionsGenerator(new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d256 });
        }

        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters>>
            {
                new TestGroupGeneratorGeneratedDataTest(_primeGen, _smallPrimeGen)
            };

            return list;
        }
    }
}
