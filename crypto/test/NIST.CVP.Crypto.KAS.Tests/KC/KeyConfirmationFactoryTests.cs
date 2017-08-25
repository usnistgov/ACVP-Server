using System;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KC
{
    [TestFixture, UnitTest]
    public class KeyConfirmationFactoryTests
    {
        private readonly KeyConfirmationFactory _subject = new KeyConfirmationFactory();

        [Test]
        [TestCase(KeyConfirmationMacType.AesCcm, 128, typeof(KeyConfirmationAesCcm))]
        [TestCase(KeyConfirmationMacType.AesCcm, 192, typeof(KeyConfirmationAesCcm))]
        [TestCase(KeyConfirmationMacType.AesCcm, 256, typeof(KeyConfirmationAesCcm))]
        [TestCase(KeyConfirmationMacType.CmacAes, 128, typeof(KeyConfirmationCmac))]
        [TestCase(KeyConfirmationMacType.HmacSha2D224, 0, typeof(KeyConfirmationHmac))]
        [TestCase(KeyConfirmationMacType.HmacSha2D256, 0, typeof(KeyConfirmationHmac))]
        [TestCase(KeyConfirmationMacType.HmacSha2D384, 0, typeof(KeyConfirmationHmac))]
        [TestCase(KeyConfirmationMacType.HmacSha2D512, 0, typeof(KeyConfirmationHmac))]
        public void ShouldReturnCorrectInstance(KeyConfirmationMacType macType, int keyLength, Type expectedType)
        {
            IKeyConfirmation result = null;

            if (macType == KeyConfirmationMacType.AesCcm)
            {
                KeyConfirmationParametersAesCcm p = new KeyConfirmationParametersAesCcm(0, 0, 0, macType, keyLength,
                    0, null, null, null, null, null, null);

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
        [TestCase(KeyConfirmationMacType.AesCcm, 0)]
        [TestCase(KeyConfirmationMacType.AesCcm, 1)]
        [TestCase(KeyConfirmationMacType.AesCcm, 2)]
        public void ShouldThrowWithInvalidKeyLengthCcm(KeyConfirmationMacType macType, int keyLength)
        {
            var p = new KeyConfirmationParametersAesCcm(0, 0, 0, macType, keyLength,
                    0, null, null, null, null, null, null);
            
            Assert.Throws(typeof(ArgumentException), () => _subject.GetInstance(p));
        }

        [Test]
        [TestCase(KeyConfirmationMacType.CmacAes, 0)]
        [TestCase(KeyConfirmationMacType.CmacAes, 1)]
        [TestCase(KeyConfirmationMacType.CmacAes, 2)]
        public void ShouldThrowWithInvalidKeyLengthCmac(KeyConfirmationMacType macType, int keyLength)
        {
            var p = new KeyConfirmationParameters(0, 0, 0, macType, keyLength,
                0, null, null, null, null, null);

            Assert.Throws(typeof(ArgumentException), () => _subject.GetInstance(p));
        }
    }
}
