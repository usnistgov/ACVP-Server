using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ParallelHash.Tests
{
    [TestFixture, FastCryptoTest]
    public class ParallelHashFactoryTests
    {
        [Test]
        [TestCase(32, 256, true)]
        [TestCase(1000, 512, true)]
        [TestCase(65536, 256, true)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestLength, int capacity, bool xof)
        {
            var hashFunction = new HashFunction(digestLength, capacity, xof);

            var subject = new ParallelHashFactory();
            var result = subject.GetParallelHash(hashFunction);

            Assert.IsInstanceOf<ParallelHashWrapper>(result);
        }

        [Test]
        [TestCase(32, 224, true)]
        [TestCase(15, 256, true)]
        [TestCase(65537, 512, true)]
        [TestCase(224, 448, false)]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestLength, int capacity, bool xof)
        {
            var hashFunction = new HashFunction(digestLength, capacity, xof);

            var subject = new ParallelHashFactory();
            Assert.Throws<ArgumentException>(() => subject.GetParallelHash(hashFunction));
        }
    }
}
