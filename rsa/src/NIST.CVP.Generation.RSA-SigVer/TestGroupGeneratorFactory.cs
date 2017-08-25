using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
