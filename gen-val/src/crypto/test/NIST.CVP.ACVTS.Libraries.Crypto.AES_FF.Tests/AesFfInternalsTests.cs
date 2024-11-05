using System.Numerics;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_FF.Tests
{
    [TestFixture, FastCryptoTest]
    public class AesFfInternalsTests
    {
        private readonly AesFfInternals _subject = new AesFfInternals(
            new Mock<IBlockCipherEngineFactory>().Object,
            new Mock<IModeBlockCipherFactory>().Object);

        [Test]
        [TestCase(5, "0 0 0 1 1 0 1 0", 755)]
        public void ShouldNumRadixCorrectly(int radix, string xStr, int expected)
        {
            var result = _subject.Num(radix, new NumeralString(xStr));

            Assert.That(result, Is.EqualTo((BigInteger)expected));
        }

        [Test]
        [TestCase("10000000", 128)]
        public void ShouldNumCorrectly(string xStr, int expected)
        {
            var x = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(xStr).Reverse());

            var result = _subject.Num(x);

            Assert.That(result, Is.EqualTo((BigInteger)expected));
        }

        [Test]
        [TestCase(4, 12, 559, new int[] { 0, 3, 10, 7 })]
        public void ShouldStrCorrectly(int m, int radix, int x, int[] expected)
        {
            var result = _subject.Str(radix, m, x);

            for (var i = 0; i <= m - 1; i++)
            {
                Assert.That(result.Numbers[i], Is.EqualTo(expected[i]));
            }
        }

        [Test]
        [TestCase(new int[] { 1, 3, 5, 7, 9 }, new int[] { 9, 7, 5, 3, 1 })]
        public void ShouldRevCorrectly(int[] x, int[] expected)
        {
            var result = _subject.Rev(new NumeralString(x));

            Assert.Multiple(() =>
            {
                for (var i = 0; i <= expected.Length - 1; i++)
                {
                    Assert.That(result.Numbers[i], Is.EqualTo(expected[i]));
                }
            });
        }

        [Test]
        [TestCase(new byte[] { 1, 2, 3 }, new byte[] { 3, 2, 1 })]
        public void ShouldRevBCorrectly(byte[] x, byte[] expected)
        {
            var result = _subject.RevB(new BitString(x));

            Assert.That(result, Is.EqualTo(new BitString(expected)));
        }
    }
}
