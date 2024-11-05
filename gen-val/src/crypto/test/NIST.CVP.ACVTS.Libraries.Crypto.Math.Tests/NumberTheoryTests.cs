using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Math.Tests
{
    [TestFixture, FastCryptoTest]
    public class NumberTheoryTests
    {
        #region MillerRabin
        [Test]
        [TestCase(true, 1, "07")]
        [TestCase(true, 50, "e021757c777288dacfe67cb2e59dc02c70a8cebf56262336592c18dcf466e0a4ed405318ac406bd79eca29183901a557db556dd06f7c6bea175dcb8460b6b1bc05832b01eedf86463238b7cb6643deef66bc4f57bf8ff7ec7c4b8a8af14f478980aabedd42afa530ca47849f0151b7736aa4cd2ff37f322a9034de791ebe3f51")]
        [TestCase(true, 50, "ed1571a9e0cd4a42541284a9f98b54a6af67d399d55ef888b9fe9ef76a61e892c0bfbb87544e7b24a60535a65de422830252b45d2033819ca32b1a9c4413fa721f4a24ebb5510ddc9fd6f4c09dfc29cb9594650620ff551a62d53edc2f8ebf10beb86f483d463774e5801f3bb01c4d452acb86ecfade1c7df601cab68b065275")]
        [TestCase(false, 50, "e534f4a4eb86ff9ace08a0b446faf3e20c22a0166057507e4f5f07332d5c0878a50798857d5e9946e3f8ef8a1021481bb0c94631f9ad8427df620ec9ca585cab3082222279f41bc40e2ccdc160dbc410c52662699ae16b27b2c9d2bf14e99083920a448ba4e5d3d11e1ab7777613959c07fb213be26f2cb7ea8a759af082f6c5")]
        [TestCase(false, 50, "d75a4a175c161cf03da85eb1679f80115e48097090032abd73170db1522754682299e450f5eda45496b33ec92274d5e311bb28ebb6102ebe93e497882ed80a9f254886fe188380f7ba02110c9781fe6b1ab22ab145f6ca1a1c0526427b38e13e1792df72372a411e78c3e57eabc19b91c9e7fa492afcda00986df8c61e2cab3f")]
        [TestCase(true, 80, "8d28148505135d7d6a1ab5075553e3516a3e0eea9158926eded37eca42a5e321")]
        public void ShouldMillerRabinCorrectly(bool expectedResult, int iterations, string hex)
        {
            var bigInt = new BitString(hex).ToPositiveBigInteger();

            var result = NumberTheory.MillerRabin(bigInt, iterations);
            Assert.That(result, Is.EqualTo(expectedResult));
        }
        #endregion MillerRabin

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
            Assert.That(NumberTheory.GCD(a, b), Is.EqualTo(expectedResult));
        }
        #endregion GCD

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
            Assert.That(NumberTheory.LCM(a, b), Is.EqualTo(expectedResult));
        }
        #endregion LCM

        #region Pow2
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(2048)]
        public void ShouldPow2Correctly(int exp)
        {
            Assert.That(NumberTheory.Pow2(exp), Is.EqualTo(BigInteger.Pow(2, exp)));
        }
        #endregion Pow2
    }
}
