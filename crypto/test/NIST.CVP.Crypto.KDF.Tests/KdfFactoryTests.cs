using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KDF.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KDF.Tests
{
    [TestFixture, FastCryptoTest]
    public class KdfFactoryTests
    {
        private KdfFactory _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new KdfFactory();
        }

        [Test]
        [TestCase(MacModes.CMAC_AES128, typeof(CmacAes))]
        [TestCase(MacModes.CMAC_AES192, typeof(CmacAes))]
        [TestCase(MacModes.CMAC_AES256, typeof(CmacAes))]
        [TestCase(MacModes.CMAC_TDES, typeof(CmacTdes))]
        [TestCase(MacModes.HMAC_SHA1, typeof(Hmac))]
        [TestCase(MacModes.HMAC_SHA224, typeof(Hmac))]
        [TestCase(MacModes.HMAC_SHA256, typeof(Hmac))]
        [TestCase(MacModes.HMAC_SHA384, typeof(Hmac))]
        [TestCase(MacModes.HMAC_SHA512, typeof(Hmac))]
        public void ShouldReturnProperMacInstance(MacModes macType, Type expectedType)
        {
            var result = _subject.GetMacInstance(macType);
            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidMacEnum()
        {
            var i = -1;
            var badMacType = (MacModes)i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetMacInstance(badMacType));
        }

        [Test]
        [TestCase(KdfModes.Counter, typeof(CounterKdf))]
        [TestCase(KdfModes.Feedback, typeof(FeedbackKdf))]
        [TestCase(KdfModes.Pipeline, typeof(PipelineKdf))]
        public void ShouldReturnProperKdfInstance(KdfModes kdfType, Type expectedType)
        {
            var result = _subject.GetKdfInstance(kdfType, MacModes.CMAC_AES128, CounterLocations.None);
            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidKdfEnum()
        {
            var i = -1;
            var badKdfType = (KdfModes)i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetKdfInstance(badKdfType, MacModes.CMAC_AES128, CounterLocations.None));
        }
    }
}
