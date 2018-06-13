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
        [TestCase(32, 256, true)]
        [TestCase(1000, 512, true)]
        [TestCase(65536, 256, true)]
        [TestCase(224, 448, false)]
        [TestCase(384, 768, false)]
        [TestCase(512, 1024, false)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestSize, int capacity, bool xof)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                XOF = xof
            };

            var subject = new SHA3Factory();
            var result = subject.GetSHA(hashFunction);

            Assert.IsInstanceOf<SHA3Wrapper>(result);
        }

        [Test]
        [TestCase(32, 224, true)]
        [TestCase(15, 256, true)]
        [TestCase(65537, 512, true)]
        [TestCase(224, 256, false)]
        [TestCase(255, 512, false)]
        [TestCase(512, 1023, false)]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestSize, int capacity, bool xof)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                XOF = xof
            };

            var subject = new SHA3Factory();
            Assert.Throws<ArgumentException>(() => subject.GetSHA(hashFunction));
        }
    }
}
