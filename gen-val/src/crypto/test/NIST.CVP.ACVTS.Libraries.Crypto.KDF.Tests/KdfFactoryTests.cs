using System;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KDF.Tests
{
    [TestFixture, FastCryptoTest]
    public class KdfFactoryTests
    {
        private KdfFactory _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper()));
        }

        [Test]
        [TestCase(MacModes.CMAC_AES128, typeof(CmacAes))]
        [TestCase(MacModes.CMAC_AES192, typeof(CmacAes))]
        [TestCase(MacModes.CMAC_AES256, typeof(CmacAes))]
        [TestCase(MacModes.CMAC_TDES, typeof(CmacTdes))]
        [TestCase(MacModes.HMAC_SHA1, typeof(NativeHmac))]
        [TestCase(MacModes.HMAC_SHA224, typeof(NativeHmac))]
        [TestCase(MacModes.HMAC_SHA256, typeof(NativeHmac))]
        [TestCase(MacModes.HMAC_SHA384, typeof(NativeHmac))]
        [TestCase(MacModes.HMAC_SHA512, typeof(NativeHmac))]
        [TestCase(MacModes.KMAC_128, typeof(Kmac))]
        [TestCase(MacModes.KMAC_256, typeof(Kmac))]
        public void ShouldReturnProperMacInstance(MacModes macType, Type expectedType)
        {
            var result = _subject.GetMacInstance(macType);
            Assert.That(result, Is.InstanceOf(expectedType));
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidMacEnum()
        {
            var i = -1;
            var badMacType = (MacModes)i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetMacInstance(badMacType));
        }

        [Test]
        [TestCase(KdfModes.Counter, MacModes.CMAC_AES128, typeof(CounterKdf))]
        [TestCase(KdfModes.Feedback, MacModes.CMAC_AES128, typeof(FeedbackKdf))]
        [TestCase(KdfModes.Pipeline, MacModes.CMAC_AES128, typeof(PipelineKdf))]
        [TestCase(KdfModes.Kmac, MacModes.KMAC_128, typeof(KmacKdf))]
        public void ShouldReturnProperKdfInstance(KdfModes kdfType, MacModes macMode, Type expectedType)
        {
            var result = _subject.GetKdfInstance(kdfType, macMode, CounterLocations.None);
            Assert.That(result, Is.InstanceOf(expectedType));
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
