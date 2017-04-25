using System.Numerics;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests
{
    [TestFixture]
    public class NumberTheoryTests
    {
        #region GCD
        private static object[] GCDSource =
        {
            new object[]
            {
                (BigInteger)1657498465164987465,
                (BigInteger)654651,
                (BigInteger)3
            },
            new object[]
            {
                (BigInteger)361682062,
                (BigInteger)18132085529,
                (BigInteger)14687
            },
            new object[]
            {
                (BigInteger)1111235916285193,
                (BigInteger)9999999900000001,
                (BigInteger)1
            },
            new object[]
            {
                (BigInteger)1333333333333333,
                (BigInteger)11111333333333,
                (BigInteger)1
            }
        };

        [Test]
        [TestCaseSource(nameof(GCDSource))]
        public void ShouldFindGCDCorrectly(BigInteger a, BigInteger b, BigInteger expectedResult)
        {
            Assert.AreEqual(expectedResult, NumberTheory.GCD(a, b));
        }
        #endregion GCD

        #region Modular Inverse
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
            Assert.AreEqual(expectedResult, NumberTheory.ModularInverse(a, m));
        }
        #endregion Modular Inverse

        #region LCM
        private static readonly BigInteger lcm1 = 3111111111113;
        private static readonly BigInteger lcm2 = 85662309563;
        private static readonly BigInteger largeResult = lcm1 * lcm2;
        private static object[] LCMSource =
        {
            new object[]
            {
                lcm1,
                lcm2, 
                largeResult
            },
            new object[]
            {
                (BigInteger)12345,
                (BigInteger)87847,
                (BigInteger)1084471215
            },
            new object[]
            {
                (BigInteger)987654,
                (BigInteger)123,
                (BigInteger)40493814
            },
            new object[]
            {
                (BigInteger)3,
                (BigInteger)10069,
                (BigInteger)30207
            }
        };

        [Test]
        [TestCaseSource(nameof(LCMSource))]
        public void ShouldFindLCMCorrectly(BigInteger a, BigInteger b, BigInteger expectedResult)
        {
            Assert.AreEqual(expectedResult, NumberTheory.LCM(a, b));
        }
        #endregion LCM

        #region Pow
        private static readonly BigInteger pow1 = 123456789123456789;
        private static readonly BigInteger pow2 = pow1 * pow1;
        private static object[] PowSource =
        {
            new object[]
            {
                pow1,
                (BigInteger)2,
                pow2
            },
            new object[]
            {
                (BigInteger)2,
                (BigInteger)63,
                (BigInteger)9223372036854775808
            },
            new object[]
            {
                (BigInteger)3,
                (BigInteger)20,
                (BigInteger)3486784401
            },
            new object[]
            {
                (BigInteger)65535,
                (BigInteger)2,
                (BigInteger)4294836225
            }
        };

        [Test]
        [TestCaseSource(nameof(PowSource))]
        public void ShouldPowCorrectly(BigInteger a, BigInteger b, BigInteger expectedResult)
        {
            Assert.AreEqual(expectedResult, NumberTheory.Pow(a, b));
        }
        #endregion Pow

        #region Ceiling Divide
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
            Assert.AreEqual(expectedResult, NumberTheory.CeilingDivide(a, b));
        }
        #endregion Ceiling Divide
    }
}
