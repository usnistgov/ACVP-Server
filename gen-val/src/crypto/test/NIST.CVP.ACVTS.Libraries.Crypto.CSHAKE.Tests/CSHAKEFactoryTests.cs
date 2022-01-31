using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.CSHAKE.Tests
{
    [TestFixture, FastCryptoTest]
    public class CSHAKEFactoryTests
    {
        [Test]
        [TestCase(32, 256)]
        [TestCase(1000, 512)]
        [TestCase(65536, 256)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestLength, int capacity)
        {
            var hashFunction = new HashFunction(digestLength, capacity);

            var subject = new CSHAKEFactory();
            var result = subject.GetCSHAKE(hashFunction);

            Assert.IsInstanceOf<CSHAKEWrapper>(result);
        }

        [Test]
        [TestCase(32, 224)]
        [TestCase(15, 256)]
        [TestCase(65537, 512)]
        [TestCase(224, 448)]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestLength, int capacity)
        {
            var hashFunction = new HashFunction(digestLength, capacity);

            var subject = new CSHAKEFactory();
            Assert.Throws<ArgumentException>(() => subject.GetCSHAKE(hashFunction));
        }
    }
}
