using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.HKDF.Tests
{
    [TestFixture, FastCryptoTest]
    public class HkdfTests
    {
        [Test]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, "0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b0b", "000102030405060708090a0b0c", "f0f1f2f3f4f5f6f7f8f9", 42, "3cb25f25faacd57a90434f64d0362f2a2d2d0a90cf1a5a4c5db02d56ecc4c5bf34007208d5b887185865")]
        public void HkdfShouldProduceCorrectResults(ModeValues mode, DigestSizes digest, string ikm, string salt, string info, int length, string okm)
        {
            var hmac = new HmacFactory(new NativeShaFactory()).GetHmacInstance(new HashFunction(mode, digest));
            var hkdf = new Hkdf(hmac);

            var ikmBs = new BitString(ikm);
            var saltBs = new BitString(salt);
            var infoBs = new BitString(info);
            var okmBs = new BitString(okm);

            var result = hkdf.DeriveKey(saltBs, ikmBs, infoBs, length);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(okmBs, result.DerivedKey);
        }
    }
}
