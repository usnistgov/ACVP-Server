using System;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CCM.Tests
{
    [TestFixture,  FastCryptoTest]
    public class AES_CCMTests
    {
        private AES_CCM _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new AES_CCM(new AES_CCMInternals(), new RijndaelFactory(new RijndaelInternals()));
        }

        [Test]
        public void ShouldEncryptAndDecryptWithValidatedTag()
        {
            var testData = GetTestData();

            // Perform encryption operation and assert
            var encryptionResult = _subject.Encrypt(testData.Key, testData.Nonce, testData.Payload, testData.AssocData,
                128);

            Assert.IsTrue(encryptionResult.Success, $"{nameof(encryptionResult.Success)} Encrypt");
            Assert.AreEqual(testData.CipherText, encryptionResult.Result, nameof(testData.CipherText));

            // Validate the decryption operation / tag
            var decryptionResult = _subject.Decrypt(testData.Key, testData.Nonce, encryptionResult.Result,
                testData.AssocData, 128);

            Assert.IsTrue(decryptionResult.Success, $"{nameof(decryptionResult.Success)} Decrypt");
            Assert.AreEqual(testData.Payload, decryptionResult.Result);
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
            var encryptionResult = _subject.Encrypt(testData.Key, testData.Nonce, testData.Payload, testData.AssocData,
                128);

            Assert.IsTrue(encryptionResult.Success, $"{nameof(encryptionResult.Success)} Encrypt");
            Assert.AreEqual(testData.CipherText, encryptionResult.Result, nameof(testData.CipherText));

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
            var decryptionResult = _subject.Decrypt(testData.Key, testData.Nonce, newCipherText,
                testData.AssocData, 128);

            Assert.IsFalse(decryptionResult.Success, $"{nameof(decryptionResult.Success)} Decrypt");
            Assert.AreEqual(Crypto.AES_CCM.AES_CCM.INVALID_TAG_MESSAGE, decryptionResult.ErrorMessage);
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


        [Test]
        public void ShouldReturnDecryptionResultWithErrorOnException()
        {
            Mock<IAES_CCMInternals> iAES_CCMInternals = new Mock<IAES_CCMInternals>();
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            _subject = new AES_CCM(iAES_CCMInternals.Object, iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = _subject.Decrypt(
                new BitString(0),
                new BitString(0),
                new BitString(0),
                new BitString(0),
                8
            );

            Assert.IsFalse(results.Success, nameof(results));
            Assert.IsInstanceOf<SymmetricCipherResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        [Test]
        public void ShouldReturnEncryptionResultWithErrorOnException()
        {
            Mock<IAES_CCMInternals> iAES_CCMInternals = new Mock<IAES_CCMInternals>();
            Mock<IRijndaelFactory> iRijndaelFactory = new Mock<IRijndaelFactory>();
            _subject = new AES_CCM(iAES_CCMInternals.Object, iRijndaelFactory.Object);
            string exceptionMessage = "Something bad happened, sorry about that.";

            iRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Throws(new Exception(exceptionMessage));

            var results = _subject.Encrypt(
                new BitString(0),
                new BitString(0),
                new BitString(0),
                new BitString(0),
                It.IsAny<int>()
            );

            Assert.IsFalse(results.Success, nameof(results));
            Assert.IsInstanceOf<SymmetricCipherResult>(results, $"{nameof(results)} type");
            Assert.AreEqual(exceptionMessage, results.ErrorMessage, nameof(exceptionMessage));
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetLogger("AES_CCM"); }
        }
    }
}
