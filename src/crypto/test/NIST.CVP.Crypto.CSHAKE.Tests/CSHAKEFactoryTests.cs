using System;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.CSHAKE.Tests
{
    [TestFixture, FastCryptoTest]
    public class CSHAKEFactoryTests
    {
        [Test]
        [TestCase(32, 256, "", "")]
        [TestCase(1000, 512, "KMAC", "customization")]
        [TestCase(65536, 256, "", "unique")]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestSize, int capacity, string functionName, string customization)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestLength = digestSize,
                FunctionName = functionName,
                Customization = customization
            };

            var subject = new CSHAKEFactory();
            var result = subject.GetCSHAKE(hashFunction);

            Assert.IsInstanceOf<CSHAKEWrapper>(result);
        }

        [Test]
        [TestCase(32, 224, "", "")]
        [TestCase(15, 256, "", "")]
        [TestCase(65537, 512, "", "")]
        [TestCase(224, 448, "", "")]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestSize, int capacity, string functionName, string customization)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestLength = digestSize,
                FunctionName = functionName,
                Customization = customization
            };

            var subject = new CSHAKEFactory();
            Assert.Throws<ArgumentException>(() => subject.GetCSHAKE(hashFunction));
        }
    }
}
