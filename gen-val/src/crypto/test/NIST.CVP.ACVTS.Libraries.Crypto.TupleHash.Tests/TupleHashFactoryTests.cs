using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TupleHash.Tests
{
    [TestFixture, FastCryptoTest]
    public class TupleHashFactoryTests
    {
        [Test]
        [TestCase(32, 256, true)]
        [TestCase(1000, 512, true)]
        [TestCase(65536, 256, true)]
        [TestCase(65536, 256, false)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestLength, int capacity, bool xof)
        {
            var hashFunction = new HashFunction(digestLength, capacity, xof);

            var subject = new TupleHashFactory();
            var result = subject.GetTupleHash(hashFunction);

            Assert.IsInstanceOf<TupleHashWrapper>(result);
        }

        [Test]
        [TestCase(32, 224, true)]
        [TestCase(15, 256, true)]
        [TestCase(65537, 512, true)]
        [TestCase(224, 448, false)]
        public void ShouldThrowExceptionWhenInvalidHashFunction(int digestLength, int capacity, bool xof)
        {
            var hashFunction = new HashFunction(digestLength, capacity, xof);

            var subject = new TupleHashFactory();
            Assert.Throws<ArgumentException>(() => subject.GetTupleHash(hashFunction));
        }
    }
}
