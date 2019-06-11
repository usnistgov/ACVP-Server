using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Numerics;

namespace NIST.CVP.Crypto.AES_FF.Tests
{
    [TestFixture, FastCryptoTest]
    public class AesFfInternalsTests
    {
        private readonly AesFfInternals _subject = new AesFfInternals();

        [Test]
        [TestCase(5, "00011010", 755)]
        public void ShouldNumRadixCorrectly(int radix, string xStr, int expected)
        {
            var x = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(xStr));

            var result = _subject.Num(radix, x);

            Assert.AreEqual((BigInteger)expected, result);
        }

        [Test]
        [TestCase("10000000", 128)]
        public void ShouldNumCorrectly(string xStr, int expected)
        {
            var x = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(xStr));

            var result = _subject.Num(x);

            Assert.AreEqual((BigInteger)expected, result);
        }

        [Test]
        [TestCase(4, 12, 559, new int[] { 0, 3, 10, 7 })]
        public void ShouldStrCorrectly(int m, int radix, int x, int[] expected)
        {
            var result = _subject.Str(radix, m, x);

            for (var i = 0; i <= m - 1; i++)
            {
                Assert.AreEqual(BitString.To32BitString(expected[i]), result.Substring(i * 32, 32), $"i: {i}");
            }
        }

        [Test]
        [TestCase(new int[] {1,3,5,7,9}, new int[] {9,7,5,3,1})]
        public void ShouldRevCorrectly(int[] x, int[] expected)
        {
             Assert.Fail();
        }

        [Test]
        [TestCase(new byte[] {1, 2, 3}, new byte[] {3, 2, 1})]
        public void ShouldRevBCorrectly(byte[] x, byte[] expected)
        {
            var result = _subject.RevB(new BitString(x));
            
            Assert.AreEqual(new BitString(expected), result);
        }
    }
}