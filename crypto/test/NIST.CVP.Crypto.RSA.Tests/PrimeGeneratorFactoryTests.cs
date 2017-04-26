using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests
{
    [TestFixture]
    public class PrimeGeneratorFactoryTests
    {
        [Test]
        [TestCase("3.2", typeof(RandomProvablePrimeGenerator))]
        [TestCase("3.3", typeof(RandomProbablePrimeGenerator))]
        public void ShouldReturnProperPrimeGenerator(string type, Type genType)
        {
            var subject = new PrimeGeneratorFactory();
            var result = subject.GetPrimeGenerator(type);

            Assert.IsInstanceOf(genType, result);
        }
    }
}
