using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class SimpleCounterTests
    {
        private readonly AesEngine _aesEngine = new AesEngine();

        [Test]
        public void ShouldWrapTheCounterWhenAtMaxValue()
        {
            var initialValue = BitString.Ones(128);
            var subject = new AdditiveCounter(_aesEngine, initialValue);

            var firstResult = subject.GetNextIV();
            var secondResult = subject.GetNextIV();
            var thirdResult = subject.GetNextIV();

            Assert.AreEqual(initialValue, firstResult);
            Assert.AreEqual(BitString.Zeroes(128), secondResult);
            Assert.AreEqual(BitString.ConcatenateBits(BitString.Zeroes(127), BitString.One()), thirdResult);
        }

        [Test]
        public void ShouldIncreaseByOneEachCall()
        {
            var subject = new AdditiveCounter(_aesEngine, BitString.Zero());

            var prevResult = subject.GetNextIV().ToPositiveBigInteger();
            for (var i = 0; i < 1000; i++)
            {
                var curResult = subject.GetNextIV().ToPositiveBigInteger();
                Assert.AreEqual(prevResult + 1, curResult);

                prevResult = curResult;
            }
        }

        [Test]
        [TestCase("00")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        [TestCase("abcdef123456")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        public void ShouldAlwaysOfferExactly128Bits(string hex)
        {
            var firstValue = new BitString(hex);
            var subject = new AdditiveCounter(_aesEngine, firstValue);

            for (var i = 0; i < 1000; i++)
            {
                Assert.AreEqual(128, subject.GetNextIV().BitLength);
            }
        }
    }
}
