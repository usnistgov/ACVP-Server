using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KDF.Tests
{
    [TestFixture, FastCryptoTest]
    public class KmacKdfTests
    {
        private KdfFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper()));
        }
        
        [Test]
        [TestCase(MacModes.KMAC_128, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "e5780b0d3ea6f7d3a429c5706aa43a00fadbd7d49628839e3187243f456ee14e", "")]
        [TestCase(MacModes.KMAC_128, 256, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "3b1fba963cd8b0b59e8c1a6d71888b7143651af8ba0a7070c0979e2811324aa5", "4d7920546167676564204170706c69636174696f6e")]
        [TestCase(MacModes.KMAC_256, 512, "404142434445464748494a4b4c4d4e4f505152535455565758595a5b5c5d5e5f", "00010203", "20c570c31346f703c9ac36c61c03cb64c3970d0cfc787e9b79599d273a68d2f7f69d4cc3de9d104a351689f27cf6f5951f0103f33f4f24871024d9c27773a8dd", "4D7920546167676564204170706C69636174696F6E")]
        public void ShouldKMACKDFCorrectly(MacModes macMode, int l, string keyI, string inputHex, string outputHex, string customization)
        {
            var kI = new BitString(keyI);
            var x = new BitString(inputHex);
            var s = new BitString(customization);
            
            var expectedResult = new BitString(outputHex);

            var subject = _factory.GetKdfInstance(KdfModes.Kmac, macMode, CounterLocations.None);
            var result = subject.DeriveKey(kI, x, l, s);
            
            Assert.That(result.Success);
            Assert.AreEqual(expectedResult, result.DerivedKey);
        }
    }
}
