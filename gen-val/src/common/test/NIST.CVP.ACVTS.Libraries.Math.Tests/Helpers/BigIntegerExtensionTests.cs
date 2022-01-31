using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class BigIntegerExtensionTests
    {
        [Test]
        public void PositiveModuloShouldAlwaysBePositive()
        {
            var rand = new Random800_90();

            for (var i = 0; i < 100; i++)
            {
                var a = rand.GetRandomBigInteger(BigInteger.Pow(2, 1024));
                var b = rand.GetRandomBigInteger(BigInteger.Pow(2, 1024) + 1, BigInteger.Pow(2, 2048));
                var c = rand.GetRandomBigInteger(BigInteger.Pow(2, 2048));

                // b is always greater than a
                var result = (a - b).PosMod(c);
                var negativeResult = (a - b) % c;

                Assert.GreaterOrEqual(result, BigInteger.Zero);

                // result - negativeResult should be a multiple of c. 
                Assert.AreEqual(BigInteger.Zero, (result + BigInteger.Abs(negativeResult)) % c);
            }
        }

        [Test]
        [TestCase("00", 0)]
        [TestCase("01", 1)]
        [TestCase("80", 8)]
        [TestCase("01ABCD", 17)]
        [TestCase("900001", 24)]
        [TestCase("00004B", 7)]
        [TestCase("00030E4D530849CD4A0D4154425A5DE775D45B059406", 162)]
        public void ExactBitLengthShouldTakeAllBitsAfterMostSignificantBit(string hex, int expectedResult)
        {
            var value = new BitString(hex).ToPositiveBigInteger();
            Assert.AreEqual(expectedResult, value.ExactBitLength());
        }

        [Test]
        [TestCase(true, "")]
        [TestCase(false, "00")]
        public void ShouldReturnCorrectZeroRepresentationForToHex(bool zeroAsEmpty, string expectation)
        {
            var value = BigInteger.Zero;

            var result = value.ToHex(zeroAsEmpty);

            Assert.AreEqual(expectation, result);
        }

        [Test]
        [TestCase(1, "01")]
        public void ShouldReturnCorrectHexRepresentationOfBigInteger(int value, string expectation)
        {
            var valueAsBigInteger = new BigInteger(value);

            var result = valueAsBigInteger.ToHex();

            Assert.AreEqual(expectation, result);
        }

        private static object[] ModInvSource =
        {
            new object[]
            {
                (BigInteger)123456789,
                (BigInteger)98765,
                (BigInteger)14659
            },
            new object[]
            {
                (BigInteger)10069,
                (BigInteger)411379717319,
                (BigInteger)37751003953
            },
            new object[]
            {
                (BigInteger)1111235916285193,
                (BigInteger)9999999900000001,
                (BigInteger)4515344125655825
            },
            new object[]
            {
                (BigInteger)1333333333333333,
                (BigInteger)11111333333333,
                (BigInteger)6486601407832
            }
        };

        [Test]
        [TestCaseSource(nameof(ModInvSource))]
        public void ShouldFindModularInverseCorrectly(BigInteger a, BigInteger m, BigInteger expectedResult)
        {
            Assert.AreEqual(expectedResult, a.ModularInverse(m));
        }

        private static object[] DivSource =
        {
            new object[]
            {
                (BigInteger)9223372036854775808,
                (BigInteger)12345,
                (BigInteger)747134227367743
            },
            new object[]
            {
                (BigInteger)96546574376,
                (BigInteger)21654,
                (BigInteger)4458603
            },
            new object[]
            {
                (BigInteger)9,
                (BigInteger)4,
                (BigInteger)3
            },
            new object[]
            {
                (BigInteger)65535,
                (BigInteger)2,
                (BigInteger)32768
            }
        };

        [Test]
        [TestCaseSource(nameof(DivSource))]
        public void ShouldCeilingDivideCorrectly(BigInteger a, BigInteger b, BigInteger expectedResult)
        {
            Assert.AreEqual(expectedResult, a.CeilingDivide(b));
        }
    }
}
