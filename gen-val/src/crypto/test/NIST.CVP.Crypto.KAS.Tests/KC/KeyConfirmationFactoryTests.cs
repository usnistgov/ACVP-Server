using System;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KC
{
    [TestFixture,  FastCryptoTest]
    public class KeyConfirmationFactoryTests
    {
        private readonly KeyConfirmationFactory _subject = new KeyConfirmationFactory(new KeyConfirmationMacDataCreator());

        [Test]
        [TestCase(KeyAgreementMacType.AesCcm, 128, typeof(KeyConfirmationAesCcm))]
        [TestCase(KeyAgreementMacType.AesCcm, 192, typeof(KeyConfirmationAesCcm))]
        [TestCase(KeyAgreementMacType.AesCcm, 256, typeof(KeyConfirmationAesCcm))]
        [TestCase(KeyAgreementMacType.CmacAes, 128, typeof(KeyConfirmationCmac))]
        [TestCase(KeyAgreementMacType.HmacSha2D224, 0, typeof(KeyConfirmationHmac))]
        [TestCase(KeyAgreementMacType.HmacSha2D256, 0, typeof(KeyConfirmationHmac))]
        [TestCase(KeyAgreementMacType.HmacSha2D384, 0, typeof(KeyConfirmationHmac))]
        [TestCase(KeyAgreementMacType.HmacSha2D512, 0, typeof(KeyConfirmationHmac))]
        public void ShouldReturnCorrectInstance(KeyAgreementMacType macType, int keyLength, Type expectedType)
        {
            IKeyConfirmation result = null;

            if (macType == KeyAgreementMacType.AesCcm)
            {
                KeyConfirmationParameters p = new KeyConfirmationParameters(0, 0, 0, macType, keyLength,
                    0, null, null, null, null, null, new BitString(1));

                result = _subject.GetInstance(p);
            }
            else
            {
                KeyConfirmationParameters p = new KeyConfirmationParameters(0, 0, 0, macType, keyLength,
                    0, null, null, null, null, null);

                result = _subject.GetInstance(p);
            }

            Assert.IsInstanceOf(expectedType, result);
        }
        
        [Test]
        [TestCase(KeyAgreementMacType.AesCcm, 0)]
        [TestCase(KeyAgreementMacType.AesCcm, 1)]
        [TestCase(KeyAgreementMacType.AesCcm, 2)]
        public void ShouldThrowWithInvalidKeyLengthCcm(KeyAgreementMacType macType, int keyLength)
        {
            var p = new KeyConfirmationParameters(0, 0, 0, macType, keyLength,
                    0, null, null, null, null, null, new BitString(1));
            
            Assert.Throws(typeof(ArgumentException), () => _subject.GetInstance(p));
        }

        [Test]
        [TestCase(KeyAgreementMacType.CmacAes, 0)]
        [TestCase(KeyAgreementMacType.CmacAes, 1)]
        [TestCase(KeyAgreementMacType.CmacAes, 2)]
        public void ShouldThrowWithInvalidKeyLengthCmac(KeyAgreementMacType macType, int keyLength)
        {
            var p = new KeyConfirmationParameters(0, 0, 0, macType, keyLength,
                0, null, null, null, null, null);

            Assert.Throws(typeof(ArgumentException), () => _subject.GetInstance(p));
        }
    }
}
