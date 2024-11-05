using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Math;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.Helpers
{
    public class KeyValidationHelperTests
    {
        private static IEnumerable<object> _testDataEcc = new[]
        {
            new object[]
            {
                "valid 1",
                Curve.P256,
                new EccPoint(
                    new BitString("0780E3B074F252EF2EFE0536047BC7F5F9CCEFF7C2D10785DED518BEC8596ACE").ToPositiveBigInteger(),
                    new BitString("DE675E8B73F6D3B0D7158C56DE92A38792D0E82175D0001F94DD8525F4BAA133").ToPositiveBigInteger()),
                true
            },
            new object[]
            {
                "valid 2",
                Curve.P256,
                new EccPoint(
                    new BitString("51B389AE76EFBD96C1B34A89F9CBA8FA559C5F1DED4CDC9B0AEDCA563EE118F0").ToPositiveBigInteger(),
                    new BitString("D9A2C981890F46BCA3BEE9AB132B6FE4B070471DA6649CE9363220E07BA839CD").ToPositiveBigInteger()),
                true
            },
            new object[]
            {
                "invalid 1",
                Curve.P256,
                new EccPoint(
                    new BitString("AB7C0107FD79084349EFD2F2FD85F53849F04B0CE20E71512F5A3661D888C4F1").ToPositiveBigInteger(),
                    new BitString("CD56EAE880FF29C8CEF071FDF6CDE7A87057C4FBD86D7DF6675BDA221E88AF21").ToPositiveBigInteger()),
                false
            },
            new object[]
            {
                "valid 3",
                Curve.P256,
                new EccPoint(
                    new BitString("AE36EE91B1C6886BCE45D4BE8FAFAF69820584B9D21D1233BEE9975A51FBC4DE").ToPositiveBigInteger(),
                    new BitString("C4CB2D0BA6EAFAFAB96EF3C3FC7434FB2FC73B8559A5F33F8C98012A1C463116").ToPositiveBigInteger()),
                true
            }
        };

        [Test]
        [TestCaseSource(nameof(_testDataEcc))]
        public void ShouldReturnExpectedValidationResultEcc(string label, Curve curveEnum, EccPoint publicKey, bool shouldPassValidation)
        {
            var curve = new EccCurveFactory().GetCurve(curveEnum);

            var result = KeyValidationHelper.PerformEccPublicKeyValidation(curve, publicKey, false);

            Assert.That(result, Is.EqualTo(shouldPassValidation));
        }
    }
}
