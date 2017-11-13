using System;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHA2.Tests
{
    [TestFixture,  FastCryptoTest]
    public class SHAFactoryTests
    {
        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(ModeValues mode, DigestSizes size)
        {
            var hashFunction = new HashFunction
            {
                Mode = mode,
                DigestSize = size
            };

            var subject = new SHAFactory();
            var result = subject.GetSHA(hashFunction);

            Assert.IsInstanceOf<ISHABase>(result);
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d224)]
        [TestCase(ModeValues.SHA1, DigestSizes.d256)]
        [TestCase(ModeValues.SHA1, DigestSizes.d384)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512t224)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512t256)]
        [TestCase(ModeValues.SHA2, DigestSizes.d160)]
        public void ShouldThrowExceptionWithInvalidHashFunction(ModeValues mode, DigestSizes size)
        {
            var hashFunction = new HashFunction
            {
                Mode = mode,
                DigestSize = size
            };

            var subject = new SHAFactory();
            Assert.Throws<ArgumentException>(() => subject.GetSHA(hashFunction));
        }
    }
}
