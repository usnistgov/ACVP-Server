using System;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Tests.Native.Helpers
{
    [TestFixture, FastCryptoTest]
    public class LmsMathHelpersTests
    {
        private static IEnumerable<object> _data = new[]
        {
            new object[]
            {
                new BitString("1234").ToBytes(),
                7,
                1,
                BigInteger.Zero
            },
            new object[]
            {
                new BitString("1234").ToBytes(),
                0,
                4,
                BigInteger.One
            },
        };

        [Test]
        [TestCaseSource(nameof(_data))]
        public void ShouldByteArrayCoefProperly(byte[] S, int i, int w, BigInteger expected)
        {
            Assert.That(LmsHelpers.Coef(S, i, w), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(LmOtsMode.LMOTS_SHA256_N24_W1)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N24_W2)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N24_W4)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N24_W8)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N32_W1)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N32_W2)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N32_W4)]
        [TestCase(LmOtsMode.LMOTS_SHA256_N32_W8)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N24_W1)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N24_W2)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N24_W4)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N24_W8)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N32_W1)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N32_W2)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N32_W4)]
        [TestCase(LmOtsMode.LMOTS_SHAKE_N32_W8)]
        public void Calculate_u_v_ls(LmOtsMode mode)
        {
            var attribute = AttributesHelper.GetLmOtsAttribute(mode);

            /*
			 u = ceil(8*n/w)
		     v = ceil((floor(lg((2^w - 1) * u)) + 1) / w)
		     
		     ls = 16 - (v * w)
			 */
            var u = (8 * attribute.N).CeilingDivide(attribute.W);
            var v = System.Math.Ceiling(
                System.Math.Floor((
                    System.Math.Log2(
                        (System.Math.Pow(
                            2, attribute.W)
                        - 1) * u)
                    ) + 1)
                / attribute.W);

            var ls = 16 - (v * attribute.W);

            Console.WriteLine($"u: {u}");
            Console.WriteLine($"v: {v}");
            Console.WriteLine($"ls: {ls}");
        }
    }
}
