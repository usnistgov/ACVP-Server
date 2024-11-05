using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.Helpers
{
    [TestFixture, FastCryptoTest]
    public class EnumMappingTests
    {
        [Test]
        [TestCase(KeyAgreementMacType.HmacSha2D224, ModeValues.SHA2, DigestSizes.d224)]
        [TestCase(KeyAgreementMacType.HmacSha2D256, ModeValues.SHA2, DigestSizes.d256)]
        [TestCase(KeyAgreementMacType.HmacSha2D384, ModeValues.SHA2, DigestSizes.d384)]
        [TestCase(KeyAgreementMacType.HmacSha2D512, ModeValues.SHA2, DigestSizes.d512)]
        public void ShouldSetCorrectHashFunctionAttributesFromHmac(KeyAgreementMacType keyAgreementMacType, ModeValues expectedModeValue, DigestSizes expectedDigestSize)
        {
            ModeValues mode = ModeValues.SHA1;
            DigestSizes digestSize = DigestSizes.NONE;

            EnumMapping.GetHashFunctionOptions(keyAgreementMacType, ref mode, ref digestSize);

            Assert.That(mode, Is.EqualTo(expectedModeValue), nameof(expectedModeValue));
            Assert.That(digestSize, Is.EqualTo(expectedDigestSize), nameof(expectedDigestSize));
        }

        [Test]
        [TestCase(KeyAgreementMacType.AesCcm)]
        [TestCase(KeyAgreementMacType.CmacAes)]
        public void ShouldThrowArgumentExceptionOnInvalidMacType(KeyAgreementMacType keyAgreementMacType)
        {
            ModeValues mode = ModeValues.SHA1;
            DigestSizes digestSize = DigestSizes.NONE;

            Assert.Throws(typeof(ArgumentException), () => EnumMapping.GetHashFunctionOptions(keyAgreementMacType, ref mode, ref digestSize));
        }
    }
}
