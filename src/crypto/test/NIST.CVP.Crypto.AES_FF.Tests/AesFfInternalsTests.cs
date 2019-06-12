using Moq;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_FF.Tests
{
    [TestFixture, FastCryptoTest]
    public class AesFfInternalsTests
    {
        private readonly AesFfInternals _subject = new AesFfInternals(
            new Mock<IBlockCipherEngineFactory>().Object,
            new Mock<IModeBlockCipherFactory>().Object);

        [Test]
        [TestCase(5, "0 0 0 1 1 0 1 0", 755)]
        public void ShouldNumRadixCorrectly(short radix, string xStr, short expected)
        {
            var result = _subject.Num(radix, new NumeralString(xStr));

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("10000000", 128)]
        public void ShouldNumCorrectly(string xStr, short expected)
        {
            var x = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(xStr));

            var result = _subject.Num(x);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(4, 12, 559, new short[] { 0, 3, 10, 7 })]
        public void ShouldStrCorrectly(short m, short radix, short x, short[] expected)
        {
            var result = _subject.Str(radix, m, x);

            for (var i = 0; i <= m - 1; i++)
            {
                Assert.AreEqual(expected[i], result.Numbers[i]);
            }
        }

        [Test]
        [TestCase(new short[] { 1, 3, 5, 7, 9 }, new short[] { 9, 7, 5, 3, 1 })]
        public void ShouldRevCorrectly(short[] x, short[] expected)
        {
            var result = _subject.Rev(new NumeralString(x));

            Assert.Multiple(() =>
            {
                for (var i = 0; i <= expected.Length - 1; i++)
                {
                    Assert.AreEqual(expected[i], result.Numbers[i]);
                }
            });
        }

        [Test]
        [TestCase(new byte[] { 1, 2, 3 }, new byte[] { 3, 2, 1 })]
        public void ShouldRevBCorrectly(byte[] x, byte[] expected)
        {
            var result = _subject.RevB(new BitString(x));

            Assert.AreEqual(new BitString(expected), result);
        }
    }
}