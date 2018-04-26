using System;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KeyWrap.Tests
{
    [TestFixture,  FastCryptoTest]
    public class KeyWrapAesTests
    {
        private Mock<IAES_ECB> _algo;
        private KeyWrapAes _subject;
        
        [SetUp]
        public void Setup()
        {
            _algo = new Mock<IAES_ECB>();
            _algo
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new SymmetricCipherResult(new BitString(128)));
            _algo
                .Setup(s => s.BlockDecrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new SymmetricCipherResult(new BitString(128)));
            _subject = new KeyWrapAes(_algo.Object);
        }

        [Test]
        public void ShouldThrowArgumentExceptionIfPayloadUnder128BitBlockSize()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => _subject.Encrypt(new BitString(128), new BitString(64), false)
            );
        }

        [Test]
        public void ShouldThrowArgumentExceptionIfPayloadNotModulus64()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => _subject.Encrypt(new BitString(128), new BitString(264), false)
            );
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenKeySizeInvalid()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => _subject.Encrypt(new BitString(264), new BitString(128), false)
            );
        }

        [Test]
        [TestCase(128)]
        [TestCase(192)]
        [TestCase(256)]
        [TestCase(1024)]
        public void ShouldInvokeAesEncryptAtLeastTwelveTimes(int payloadSize)
        {
            BitString payload = new BitString(payloadSize);

            int expectedNumberOfInvocations = 6 * (((payloadSize + KeyWrapAes.Icv1.BitLength) / 64) - 1);

            _subject.Encrypt(new BitString(128), payload, false);

            _algo.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false), Times.Exactly(expectedNumberOfInvocations), $"{expectedNumberOfInvocations} invokations of {nameof(_algo.Object.BlockEncrypt)} expected.");
        }
        
        [Test, FastCryptoTest]
        [TestCase("CASE 1: KW with AES, wrap 128 bits of plaintext with a 128-bit key", KeyWrapType.AES_KW, false, true, "000102030405060708090A0B0C0D0E0F", "00112233445566778899AABBCCDDEEFF", "1fa68b0a8112b447AEF34BD8FB5A7B829D3E862371D2CFE5")]
        [TestCase("CASE 2: KW with AES, wrap 128 bits of plaintext with a 192-bit key", KeyWrapType.AES_KW, false, true, "000102030405060708090A0B0C0D0E0F1011121314151617", "00112233445566778899AABBCCDDEEFF", "96778b25ae6ca435F92B5B97C050AED2468AB8A17AD84E5D")]
        [TestCase("CASE 3: KW with AES, wrap 128 bits of plaintext with a 256-bit key", KeyWrapType.AES_KW, false, true, "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", "00112233445566778899AABBCCDDEEFF", "64e8c3f9ce0f5ba263E9777905818A2A93C8191E7D6E8AE7")]
        [TestCase("CASE 4: KW with AES, wrap 192 bits of plaintext with a 192-bit key", KeyWrapType.AES_KW, false, true, "000102030405060708090A0B0C0D0E0F1011121314151617", "00112233445566778899AABBCCDDEEFF0001020304050607", "031d33264e15d33268F24EC260743EDCE1C6C7DDEE725A936BA814915C6762D2")]
        [TestCase("CASE 5: KW with AES, wrap 192 bits of plaintext with a 256-bit key", KeyWrapType.AES_KW, false, true, "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", "00112233445566778899AABBCCDDEEFF0001020304050607", "a8f9bc1612c68b3fF6E6F4FBE30E71E4769C8B80A32CB8958CD5D17D6B254DA1")]
        [TestCase("CASE 6: KW with AES, wrap 256 bits of plaintext with a 256-bit key", KeyWrapType.AES_KW, false, true, "000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", "00112233445566778899AABBCCDDEEFF000102030405060708090A0B0C0D0E0F", "28c9f404c4b810f4CBCCB35CFB87F8263F5786E2D80ED326CBC7F0E71A99F43BFB988B9B7A02DD21")]
        [TestCase("Case 15: KW_AE, inverse cipher function", KeyWrapType.AES_KW, true, true, "761d0145907fb5064923a302b7f7924b", "ef6536af7de618b50ec11b5d719585a669dacdfd16b7067f", "046acacc2c25fd90bf15947ae9000d3a8c025684aad58bcdf970c19997207acb")]
        [TestCase("Case 16: KW_AD, inverse cipher function", KeyWrapType.AES_KW, true, false, "e8b58fb37ca9c313a679de74f2dbedfa", "11a608b60d46bc008e98b89a37966cc11c40ad543a75fe93", "")]
        public void ShouldReturnExpectedValue(string testLabel, KeyWrapType keyWrapType, bool useInverseCipher, bool successfulAuthenticate, string kString, string pString, string cExpectedString)
        {
            BitString K = new BitString(kString);
            BitString P = new BitString(pString);
            BitString expectedC = new BitString(cExpectedString);

            _subject = new KeyWrapAes(new AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals())));
            var actualC = _subject.Encrypt(K, P, useInverseCipher);

            // Mangle the actualC returned when it should not decrypt successfully
            if (!successfulAuthenticate)
            {
                Random800_90 rand = new Random800_90();
                actualC = new SymmetricCipherResult(rand.GetDifferentBitStringOfSameSize(actualC.Result));
            }

            var decrypt = _subject.Decrypt(K, actualC.Result, useInverseCipher);

            if (!successfulAuthenticate)
            {
                Assert.IsFalse(decrypt.Success);
                return;
            }

            Assert.AreEqual(expectedC.ToHex(), actualC.Result.ToHex(), "encrypt compare");
            Assert.AreEqual(P.ToHex(), decrypt.Result.ToHex(), "decrypt compare");
        }
    }
}
