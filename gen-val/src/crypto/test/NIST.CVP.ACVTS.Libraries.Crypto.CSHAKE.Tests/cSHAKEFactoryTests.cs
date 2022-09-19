using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE.Tests
{
    [TestFixture, FastCryptoTest]
    public class cSHAKEFactoryTests
    {
        [Test]
        [TestCase(32, 256)]
        [TestCase(1000, 512)]
        [TestCase(65536, 256)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestLength, int capacity)
        {
            var hashFunction = new HashFunction(digestLength, capacity);

            var subject = new cSHAKEFactory();
            var result = subject.GetcSHAKE(hashFunction);

            Assert.IsInstanceOf<cSHAKEWrapper>(result);
        }

        [Test]
        [TestCase(32, 224)]
        [TestCase(15, 256)]
        [TestCase(65537, 512)]
        [TestCase(224, 448)]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestLength, int capacity)
        {
            var hashFunction = new HashFunction(digestLength, capacity);

            var subject = new cSHAKEFactory();
            Assert.Throws<ArgumentException>(() => subject.GetcSHAKE(hashFunction));
        }
    }
}
