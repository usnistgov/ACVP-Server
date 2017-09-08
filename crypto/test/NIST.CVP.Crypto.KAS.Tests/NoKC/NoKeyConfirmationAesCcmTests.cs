using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.NoKC
{
    [TestFixture, FastIntegrationTest]
    public class NoKeyConfirmationAesCcmTests
    {
        private NoKeyConfirmationAesCcm _subject;
        private readonly AES_CCM.AES_CCM _algo = new AES_CCM.AES_CCM(new AES_CCMInternals(), new RijndaelFactory(new RijndaelInternals()));
        
        private static object[] _testData = new object[]
        {
            new object[]
            {
                // Key length
                128,
                // ccmNonce length
                56,
                // mac length
                64,
                // DKM
                new BitString("0edc6e20af7c11ef41457deb6db9ad0b"),
                // Nonce
                new BitString("1d33ef74a9f3c6e7ab1dfa77b40e4191"),
                // CCM Nonce
                new BitString("6526d522a19f25"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d6573736167651d33ef74a9f3c6e7ab1dfa77b40e4191"),
                // Expected MAC
                new BitString("9a0ade2f4b22599e")
            },
            new object[]
            {
                // Key length
                128,
                // ccmNonce length
                56,
                // mac length
                64,
                // DKM
                new BitString("36e4156ffd2ea30ffbc4bafb31eff2d1"),
                // Nonce
                new BitString("4ce9948299a0d3d1c80222ca6ca7c276"),
                // CCM Nonce
                new BitString("18169075b94216"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d6573736167654ce9948299a0d3d1c80222ca6ca7c276"),
                // Expected MAC
                new BitString("89d63e0bccdefa82")
            },
            new object[]
            {
                // Key length
                128,
                // ccmNonce length
                56,
                // tag length
                64,
                // DKM
                new BitString("3e79800e91bad653e4dc38dc6bbac4ee"),
                // Nonce
                new BitString("da3dc604ebc14f641a864ec22c08d734"),
                // CCM Nonce
                new BitString("27db08d6d842a2"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765da3dc604ebc14f641a864ec22c08d734"),
                // Expected MAC
                new BitString("40ef2a90111b37d8"),
            },
            new object[]
            {
                // Key length
                128,
                // ccmNonce length
                56,
                // tag length
                64,
                // DKM
                new BitString("7f5944fe6f6d86aca13b9efd91f9e3f3"),
                // Nonce
                new BitString("be0e446331fbec5ac31f32b27bf6f0ee"),
                // CCM Nonce
                new BitString("71db2acab2515b"),
                // Expected MAC data
                new BitString("5374616e646172642054657374204d657373616765be0e446331fbec5ac31f32b27bf6f0ee"),
                // Expected MAC
                new BitString("c7414f450c41abe5"),
            }
            //new object[]
            //{
            //    // Key length
            //    128,
            //    // ccmNonce length
            //    56,
            //    // tag length
            //    64,
            //    // DKM
            //    new BitString(""),
            //    // Nonce
            //    new BitString(""),
            //    // CCM Nonce
            //    new BitString(""),
            //    // Expected MAC data
            //    new BitString(""),
            //    // Expected MAC
            //    new BitString(""),
            //}
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldMacCorrectly(int keyLength, int ccmNonceLength, int macLength, BitString dkm, BitString nonce, BitString ccmNonce, BitString expectedMacData, BitString expectedMac)
        {
            NoKeyConfirmationParameters p =
                new NoKeyConfirmationParameters(KeyAgreementMacType.AesCcm, macLength, dkm, nonce, ccmNonce);
            _subject = new NoKeyConfirmationAesCcm(p, _algo);

            var result = _subject.ComputeMac();

            Assume.That(result.Success, nameof(result.Success));
            Assert.AreEqual(expectedMacData.ToHex(), result.MacData.ToHex(), nameof(result.MacData));
            Assert.AreEqual(expectedMac.ToHex(), result.Mac.ToHex(), nameof(result.Mac));
        }
    }
}
