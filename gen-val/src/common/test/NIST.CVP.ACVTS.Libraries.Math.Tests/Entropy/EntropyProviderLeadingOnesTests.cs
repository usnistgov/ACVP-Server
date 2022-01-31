using System;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Entropy
{
    [TestFixture, UnitTest]
    public class EntropyProviderLeadingOnesTests
    {
        private EntropyProviderLeadingOnes _subject;

        [Test]
        [TestCase(8, 8)]
        [TestCase(8, 16)]
        [TestCase(4, 1024)]
        [TestCase(17, 1024)]
        public void RandomBitsShouldHaveLeadingZeroBits(int leadingOneBits, int numberOfBits)
        {
            var count = 100; // arbitrary
            for (var i = 0; i < count; i++)
            {
                _subject = new EntropyProviderLeadingOnes(new Random800_90(), leadingOneBits);
                var result = _subject.GetEntropy(numberOfBits);

                var expectedBitString = new BitString(leadingOneBits);
                for (var j = 0; j < leadingOneBits; j++)
                {
                    expectedBitString.Bits[j] = true;
                }

                Assert.AreEqual(expectedBitString.ToHex(), result.GetMostSignificantBits(leadingOneBits).ToHex());
            }
        }

        [Test]
        [TestCase(4, 0)]
        [TestCase(4, 1)]
        public void ShouldThrowWhenAttemptingToPullLessBitsThanLeadingZeroBits(int leadingOneBits, int numberOfBits)
        {
            _subject = new EntropyProviderLeadingOnes(new Random800_90(), leadingOneBits);
            Assert.Throws<ArgumentOutOfRangeException>(() => _subject.GetEntropy(numberOfBits));
        }
    }
}
