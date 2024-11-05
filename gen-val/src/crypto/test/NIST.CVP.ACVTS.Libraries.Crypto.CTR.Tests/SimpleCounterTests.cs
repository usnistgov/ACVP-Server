using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.CTR.Tests
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

            Assert.That(firstResult, Is.EqualTo(initialValue));
            Assert.That(secondResult, Is.EqualTo(BitString.Zeroes(128)));
            Assert.That(thirdResult, Is.EqualTo(BitString.ConcatenateBits(BitString.Zeroes(127), BitString.One())));
        }

        [Test]
        public void ShouldIncreaseByOneEachCall()
        {
            var subject = new AdditiveCounter(_aesEngine, BitString.Zero());

            var prevResult = subject.GetNextIV().ToPositiveBigInteger();
            for (var i = 0; i < 1000; i++)
            {
                var curResult = subject.GetNextIV().ToPositiveBigInteger();
                Assert.That(curResult, Is.EqualTo(prevResult + 1));

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
                Assert.That(subject.GetNextIV().BitLength, Is.EqualTo(128));
            }
        }
    }
}
