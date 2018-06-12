using System;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHA3.Tests
{
    [TestFixture,  FastCryptoTest]
    public class SHA3FactoryTests
    {
        [Test]
        [TestCase(32, 256, Output.XOF)]
        [TestCase(1000, 512, Output.XOF)]
        [TestCase(65536, 256, Output.XOF)]
        [TestCase(224, 448, Output.CONSTANT)]
        [TestCase(384, 768, Output.CONSTANT)]
        [TestCase(512, 1024, Output.CONSTANT)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestSize, int capacity, Output outputType)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                OutputType = outputType
            };

            var subject = new SHA3Factory();
            var result = subject.GetSHA(hashFunction);

            Assert.IsInstanceOf<SHA3Wrapper>(result);
        }

        [Test]
        [TestCase(32, 224, Output.XOF)]
        [TestCase(15, 256, Output.XOF)]
        [TestCase(65537, 512, Output.XOF)]
        [TestCase(224, 256, Output.CONSTANT)]
        [TestCase(255, 512, Output.CONSTANT)]
        [TestCase(512, 1023, Output.CONSTANT)]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestSize, int capacity, Output outputType)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                OutputType = outputType
            };

            var subject = new SHA3Factory();
            Assert.Throws<ArgumentException>(() => subject.GetSHA(hashFunction));
        }
    }
}
