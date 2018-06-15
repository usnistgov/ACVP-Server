using System;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TupleHash.Tests
{
    [TestFixture, FastCryptoTest]
    public class TupleHashFactoryTests
    {
        [Test]
        [TestCase(32, 256, true)]
        [TestCase(1000, 512, true)]
        [TestCase(65536, 256, true)]
        [TestCase(65536, 256, false)]
        public void ShouldProduceValidAlgorithmWithValidHashFunction(int digestSize, int capacity, bool XOF)
        {
            var hashFunction = new HashFunction()
            {
                Capacity = capacity,
                DigestSize = digestSize,
                XOF = XOF
            };

            var subject = new TupleHashFactory();
            var result = subject.GetTupleHash(hashFunction);

            Assert.IsInstanceOf<TupleHashWrapper>(result);
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

            var subject = new TupleHashFactory();
            Assert.Throws<ArgumentException>(() => subject.GetTupleHash(hashFunction));
        }
    }
}
