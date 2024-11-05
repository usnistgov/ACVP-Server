using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Entropy
{
    [TestFixture, UnitTest]
    public class EntropyProviderLeadingZeroesTests
    {
        private EntropyProviderLeadingZeroes _subject;

        [Test]
        [TestCase(8, 8)]
        [TestCase(8, 16)]
        [TestCase(4, 1024)]
        [TestCase(17, 1024)]
        public void RandomBitsShouldHaveLeadingZeroBits(int leadingZeroBits, int numberOfBits)
        {
            var count = 100; // arbitrary
            for (var i = 0; i < count; i++)
            {
                _subject = new EntropyProviderLeadingZeroes(new Random800_90(), leadingZeroBits);
                var result = _subject.GetEntropy(numberOfBits);

                Assert.That(result.GetMostSignificantBits(leadingZeroBits).ToPositiveBigInteger(), Is.EqualTo(BigInteger.Zero));
            }
        }

        [Test]
        [TestCase(4, 0)]
        [TestCase(4, 1)]
        public void ShouldThrowWhenAttemptingToPullLessBitsThanLeadingZeroBits(int leadingZeroBits, int numberOfBits)
        {
            _subject = new EntropyProviderLeadingZeroes(new Random800_90(), leadingZeroBits);
            Assert.Throws<ArgumentOutOfRangeException>(() => _subject.GetEntropy(numberOfBits));
        }
    }
}
