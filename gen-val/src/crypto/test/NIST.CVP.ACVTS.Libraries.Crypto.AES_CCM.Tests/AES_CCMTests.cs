using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CCM.Tests
{
    [TestFixture, FastCryptoTest]
    public class AES_CCMTests
    {
        private IAeadModeBlockCipher _newSubject;

        [SetUp]
        public void Setup()
        {
            _newSubject = new CcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_CCMInternals());
        }

        [Test]
        public void ShouldEncryptAndDecryptWithValidatedTag()
        {
            var testData = GetTestData();

            // Perform encryption operation and assert
            var encryptParam = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                testData.Nonce,
                testData.Key,
                testData.Payload,
                testData.AssocData,
                128
            );
            var encryptionResult = _newSubject.ProcessPayload(encryptParam);

            Assert.That(encryptionResult.Success, Is.True, $"{nameof(encryptionResult.Success)} Encrypt");
            Assert.That(encryptionResult.Result, Is.EqualTo(testData.CipherText), nameof(testData.CipherText));

            // Validate the decryption operation / tag
            var decryptParam = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                testData.Nonce,
                testData.Key,
                encryptionResult.Result,
                testData.AssocData,
                128
            );
            var decryptionResult = _newSubject.ProcessPayload(decryptParam);

            Assert.That(decryptionResult.Success, Is.True, $"{nameof(decryptionResult.Success)} Decrypt");
            Assert.That(decryptionResult.Result, Is.EqualTo(testData.Payload));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void ShouldFailDueToInvalidTag(int indexToChange)
        {
            var testData = GetTestData();

            // Perform encryption operation and assert
            var encryptParam = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                testData.Nonce,
                testData.Key,
                testData.Payload,
                testData.AssocData,
                128
            );
            var encryptionResult = _newSubject.ProcessPayload(encryptParam);

            Assert.That(encryptionResult.Success, Is.True, $"{nameof(encryptionResult.Success)} Encrypt");
            Assert.That(encryptionResult.Result, Is.EqualTo(testData.CipherText), nameof(testData.CipherText));

            // Change a byte value to invalidate the decryption
            var encryptionResultBytes = encryptionResult.Result.ToBytes();
            if (encryptionResultBytes.Length >= indexToChange)
            {
                if (encryptionResultBytes[indexToChange] == 255)
                {
                    encryptionResultBytes[indexToChange]--;
                }
                else
                {
                    encryptionResultBytes[indexToChange]++;
                }
            }

            var newCipherText = new BitString(encryptionResultBytes);

            // Validate the decryption operation / tag
            var decryptParam = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                testData.Nonce,
                testData.Key,
                newCipherText,
                testData.AssocData,
                128
            );
            var decryptionResult = _newSubject.ProcessPayload(decryptParam);

            Assert.That(decryptionResult.Success, Is.False, $"{nameof(decryptionResult.Success)} Decrypt");
            Assert.That(decryptionResult.ErrorMessage, Is.EqualTo(CcmBlockCipher.INVALID_TAG_MESSAGE));
        }

        [Test]
        [TestCase(
            // nonce
            "958a3ede772faf",
            // key
            "e9d1ded93334397c6b6b33ed3ec3fa6e7237aa801ed81e5e167205cbaa6c380d",
            // aad
            "4c50f81107140b0104fa0a3706427f268717435ab47a9c08b87ca9ea3db00049",
            // payload
            "6a8bbcde06daa9c8753fa1aab319977971a4e9f00b1dedd6ba26cd830b6b96a1",
            // ct
            "0b848f292b987337cc98b95a0f2c5da9b22a26386b5d75f6e78f7dd0fb89dd85b5805716e16017ec78ee44b26f28c996",
            // tag length
            128
        )]
        [TestCase(
            // nonce
            "970e636da4ab7f7b8bffa6d840",
            // key
            "c3f989b353095884677d2ac4be68d3cc",
            // aad
            "9c5e05bd3ac842d8b91c0c629e2220f9a1fd2181a7522f6af0acbc31921f9b2a",
            // payload
            "e53f83eb85f1f1566822141015c954eee07c40546ae4314b6b6f2ad4af44037d",
            // ct
            "dc5640e25839a8a0b9404f46364f5fdfe6c96d81a0db0e991e9b3d1d30e8dc1c75ccf950",
            // tag length
            32
        )]
        public void NewEngineTests(
            string nonceStr,
            string keyStr,
            string aadStr,
            string payloadStr,
            string ctStr,
            int tagLength
        )
        {
            var nonce = new BitString(nonceStr);
            var key = new BitString(keyStr);
            var aad = new BitString(aadStr);
            var payload = new BitString(payloadStr);
            var expectedCt = new BitString(ctStr);

            // Perform encryption operation and assert
            var encryptParam = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                nonce,
                key,
                payload,
                aad,
                tagLength
            );
            var encryptionResult = _newSubject.ProcessPayload(encryptParam);

            Assert.That(encryptionResult.Success, Is.True, $"{nameof(encryptionResult.Success)} Encrypt");
            Assert.That(encryptionResult.Result, Is.EqualTo(expectedCt), nameof(expectedCt));

            // Validate the decryption operation / tag
            var decryptParam = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Decrypt,
                nonce,
                key,
                encryptionResult.Result,
                aad,
                tagLength
            );
            var decryptionResult = _newSubject.ProcessPayload(decryptParam);

            Assert.That(decryptionResult.Success, Is.True, $"{nameof(decryptionResult.Success)} Decrypt");
            Assert.That(decryptionResult.Result, Is.EqualTo(payload));
        }

        private class TestData
        {
            public BitString CipherText { get; set; }
            public BitString Key { get; set; }
            public BitString AssocData { get; set; }
            public BitString Payload { get; set; }
            public BitString Nonce { get; set; }
        }

        private TestData GetTestData()
        {
            TestData resp = new TestData();

            var NonceBytes = new byte[10];
            var AssocDataBytes = new byte[16];
            var PayloadBytes = new byte[16];
            var KeyBytes = new byte[16];

            KeyBytes[0] = 0xf3; KeyBytes[1] = 0x44;
            KeyBytes[2] = 0x81; KeyBytes[3] = 0xec;
            KeyBytes[4] = 0x3c; KeyBytes[5] = 0xc6;
            KeyBytes[6] = 0x27; KeyBytes[7] = 0xba;
            KeyBytes[8] = 0xcd; KeyBytes[9] = 0x5d;
            KeyBytes[10] = 0xc3; KeyBytes[11] = 0xfb;
            KeyBytes[12] = 0x08; KeyBytes[13] = 0xf2;
            KeyBytes[14] = 0x73; KeyBytes[15] = 0xe6;

            AssocDataBytes[0] = 0x01; AssocDataBytes[1] = 0x47;
            AssocDataBytes[2] = 0x30; AssocDataBytes[3] = 0xf8;
            AssocDataBytes[4] = 0x0a; AssocDataBytes[5] = 0xc6;
            AssocDataBytes[6] = 0x25; AssocDataBytes[7] = 0xfe;
            AssocDataBytes[8] = 0x84; AssocDataBytes[9] = 0xf0;
            AssocDataBytes[10] = 0x26; AssocDataBytes[11] = 0xc6;
            AssocDataBytes[12] = 0x0b; AssocDataBytes[13] = 0xfd;
            AssocDataBytes[14] = 0x54; AssocDataBytes[15] = 0x7d;

            PayloadBytes[0] = 0x1b; PayloadBytes[1] = 0x07;
            PayloadBytes[2] = 0x7a; PayloadBytes[3] = 0x6a;
            PayloadBytes[4] = 0xf4; PayloadBytes[5] = 0xb7;
            PayloadBytes[6] = 0xf9; PayloadBytes[7] = 0x82;
            PayloadBytes[8] = 0x29; PayloadBytes[9] = 0xde;
            PayloadBytes[10] = 0x78; PayloadBytes[11] = 0x6d;
            PayloadBytes[12] = 0x75; PayloadBytes[13] = 0x16;
            PayloadBytes[14] = 0xb6; PayloadBytes[15] = 0x39;

            NonceBytes[0] = 0x01; NonceBytes[1] = 0x02;
            NonceBytes[2] = 0x03; NonceBytes[3] = 0x04;
            NonceBytes[4] = 0x05; NonceBytes[5] = 0x06;
            NonceBytes[6] = 0x07; NonceBytes[7] = 0x08;
            NonceBytes[8] = 0x09; NonceBytes[9] = 0x0a;

            byte[] expectedCtBytes = new byte[32];
            expectedCtBytes[0] = 0x57;
            expectedCtBytes[1] = 0x49;
            expectedCtBytes[2] = 0x5e;
            expectedCtBytes[3] = 0x8b;
            expectedCtBytes[4] = 0x5a;
            expectedCtBytes[5] = 0xe6;
            expectedCtBytes[6] = 0x28;
            expectedCtBytes[7] = 0xf9;
            expectedCtBytes[8] = 0x14;
            expectedCtBytes[9] = 0xf4;
            expectedCtBytes[10] = 0x78;
            expectedCtBytes[11] = 0xad;
            expectedCtBytes[12] = 0x32;
            expectedCtBytes[13] = 0x15;
            expectedCtBytes[14] = 0xea;
            expectedCtBytes[15] = 0x96;
            expectedCtBytes[16] = 0x1a;
            expectedCtBytes[17] = 0xb7;
            expectedCtBytes[18] = 0xcf;
            expectedCtBytes[19] = 0xeb;
            expectedCtBytes[20] = 0xa1;
            expectedCtBytes[21] = 0x18;
            expectedCtBytes[22] = 0x9a;
            expectedCtBytes[23] = 0x9c;
            expectedCtBytes[24] = 0xe7;
            expectedCtBytes[25] = 0x88;
            expectedCtBytes[26] = 0xe3;
            expectedCtBytes[27] = 0xd7;
            expectedCtBytes[28] = 0x86;
            expectedCtBytes[29] = 0x82;
            expectedCtBytes[30] = 0x21;
            expectedCtBytes[31] = 0x35;

            resp.CipherText = new BitString(expectedCtBytes);
            resp.Key = new BitString(KeyBytes);
            resp.AssocData = new BitString(AssocDataBytes);
            resp.Payload = new BitString(PayloadBytes);
            resp.Nonce = new BitString(NonceBytes);

            return resp;
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetLogger("AES_CCM"); }
        }
    }
}
