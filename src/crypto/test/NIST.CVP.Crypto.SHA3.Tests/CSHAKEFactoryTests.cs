using System;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHA3.Tests
{
    [TestFixture, FastCryptoTest]
    public class CSHAKEFactoryTests
    {
        [Test]
        [TestCase(32, 256, Output.cXOF)]
        [TestCase(1000, 512, Output.cXOF)]
        [TestCase(65536, 256, Output.cXOF)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestSize, int capacity, Output outputType)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                OutputType = outputType
            };

            var subject = new CSHAKEFactory();
            var result = subject.GetCSHAKE(hashFunction);

            Assert.IsInstanceOf<CSHAKEWrapper>(result);
        }

        [Test]
        [TestCase(32, 224, Output.cXOF)]
        [TestCase(15, 256, Output.cXOF)]
        [TestCase(65537, 512, Output.cXOF)]
        [TestCase(224, 448, Output.CONSTANT)]
        [TestCase(1000, 512, Output.XOF)]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestSize, int capacity, Output outputType)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                OutputType = outputType
            };

            var subject = new CSHAKEFactory();
            Assert.Throws<ArgumentException>(() => subject.GetCSHAKE(hashFunction));
        }
    }
}
