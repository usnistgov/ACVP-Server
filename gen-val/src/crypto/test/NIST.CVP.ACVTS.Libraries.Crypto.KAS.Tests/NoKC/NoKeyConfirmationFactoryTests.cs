using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.NoKC
{
    [TestFixture, FastCryptoTest]
    public class NoKeyConfirmationFactoryTests
    {
        private NoKeyConfirmationFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new NoKeyConfirmationFactory(new NoKeyConfirmationMacDataCreator());
        }

        [Test]
        [TestCase(KeyAgreementMacType.AesCcm, typeof(NoKeyConfirmationAesCcm))]
        [TestCase(KeyAgreementMacType.CmacAes, typeof(NoKeyConfirmationCmac))]
        [TestCase(KeyAgreementMacType.HmacSha2D224, typeof(NoKeyConfirmationHmac))]
        [TestCase(KeyAgreementMacType.HmacSha2D256, typeof(NoKeyConfirmationHmac))]
        [TestCase(KeyAgreementMacType.HmacSha2D384, typeof(NoKeyConfirmationHmac))]
        [TestCase(KeyAgreementMacType.HmacSha2D512, typeof(NoKeyConfirmationHmac))]
        public void ShouldReturnCorrectInstance(KeyAgreementMacType macType, Type expectedType)
        {

            NoKeyConfirmationParameters p = null;
            if (macType == KeyAgreementMacType.AesCcm)
            {
                p = new NoKeyConfirmationParameters(macType, 1, new BitString(1), new BitString(1), new BitString(1));
            }
            else
            {
                p = new NoKeyConfirmationParameters(macType, 1, new BitString(1), new BitString(1));
            }

            var result = _subject.GetInstance(p);

            Assert.That(result, Is.InstanceOf(expectedType));
        }
    }
}
