using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math.Entropy;using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests
{
    [TestFixture,  FastCryptoTest]
    public class PrimeGeneratorFactoryTests
    {
        [Test]
        [TestCase("3.2", typeof(RandomProvablePrimeGenerator))]
        [TestCase("3.3", typeof(RandomProbablePrimeGenerator))]
        [TestCase("3.4", typeof(AllProvablePrimesWithConditionsGenerator))]
        [TestCase("3.5", typeof(ProvableProbablePrimesWithConditionsGenerator))]
        [TestCase("3.6", typeof(AllProbablePrimesWithConditionsGenerator))]
        public void ShouldReturnProperPrimeGenerator(string type, Type genType)
        {
            var hashFunction = new HashFunction {Mode = ModeValues.SHA2, DigestSize = DigestSizes.d224};
            var entropyType = EntropyProviderTypes.Random;

            var subject = new PrimeGeneratorFactory(hashFunction, entropyType);
            var result = subject.GetPrimeGenerator(type);

            Assert.IsInstanceOf(genType, result);
        }
    }
}
