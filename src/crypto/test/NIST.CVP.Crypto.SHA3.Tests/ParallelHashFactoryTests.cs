using System;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHA3.Tests
{
    [TestFixture, FastCryptoTest]
    public class ParallelHashFactoryTests
    {
        [Test]
        [TestCase(32, 256, true)]
        [TestCase(1000, 512, true)]
        [TestCase(65536, 256, true)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestSize, int capacity, bool XOF)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                XOF = XOF
            };

            var subject = new ParallelHashFactory();
            var result = subject.GetParallelHash(hashFunction);

            Assert.IsInstanceOf<ParallelHashWrapper>(result);
        }

        [Test]
        [TestCase(32, 224, true)]
        [TestCase(15, 256, true)]
        [TestCase(65537, 512, true)]
        [TestCase(224, 448, false)]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestSize, int capacity, bool XOF)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                XOF = XOF
            };

            var subject = new ParallelHashFactory();
            Assert.Throws<ArgumentException>(() => subject.GetParallelHash(hashFunction));
        }
    }
}
