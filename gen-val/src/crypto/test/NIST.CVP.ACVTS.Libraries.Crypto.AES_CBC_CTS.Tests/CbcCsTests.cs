using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.CTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CBC_CTS.Tests
{
    [TestFixture, FastCryptoTest]
    public class CbcCsTests
    {
        private readonly CbcCtsBlockCipher _subjectCs1 = new CbcCtsBlockCipher(new AesEngine(), new CiphertextStealingMode1());
        private readonly CbcCtsBlockCipher _subjectCs2 = new CbcCtsBlockCipher(new AesEngine(), new CiphertextStealingMode2());
        private readonly CbcCtsBlockCipher _subjectCs3 = new CbcCtsBlockCipher(new AesEngine(), new CiphertextStealingMode3());

        private static IEnumerable<object> _testData = new[]
        {
//            new object[]
//            {
//                // label
//                "test1 fail from IUT (using server generated value",
//                // iv
//                new BitString("00000000000000000000000000000000"),
//                // pt
//                new BitString("F34481EC3CC627BACD5DC3FB08F273E6000000000000000000000000000000000000000000000000"), 
//                // key
//                new BitString("00000000000000000000000000000000"), 
//                // expected ct
//                new BitString("0C97474C3A5887D896987F20F03D5878144B1CE528C44F0F40F04701BE78F9E18AFF0A1F5095F84E")
//            },
            new object[]
            {
                // label
                "test1 fail from IUT (using iut generated value",
                // iv
                new BitString("00000000000000000000000000000000"),
                // pt
                new BitString("F34481EC3CC627BACD5DC3FB08F273E6000000000000000000000000000000000000000000000000"), 
                // key
                new BitString("00000000000000000000000000000000"), 
                // expected ct
                new BitString("0336763e966d92595a567cc9ce537f5ed2ad6ef1a3f88079814a8e7083ab12ebd9492aafc53406fc")
            },
            new object[]
            {
                // label
                "test2 pass from IUT",
                // iv
                new BitString("669B5C6813D17C721B771A412ABEB14D"),
                // pt
                new BitString("DFD77A131C9C173970136D4434402FEB5A0D5C583A9307E1AE79052B0AEC8689BB4E815AA90B5B03"), 
                // key
                new BitString("A69FE3155652F8F0D3D99533CC46FC8E"), 
                // expected ct
                new BitString("2763209E2C801E672447A44E86403344A177472F5B83E36C8AB230D3AFF37001CDE5C6812B21D46E")
            },
            new object[]
            {
                // label
                "test3 mct first round",
                // iv
                new BitString("00000000000000000000000000000000"),
                // pt
                new BitString("000000000000000000000000000000000000000000000000"), 
                // key
                new BitString("00000000000000000000000000000000"), 
                // expected ct
                new BitString("F795BD4A52E29ED713D313FA20E98DBC66E94BD4EF8A2C3B")
            },
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldEncryptCorrectly(string label, BitString iv, BitString pt, BitString key, BitString ct)
        {
            var result = _subjectCs3
                .ProcessPayload(new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, iv.GetDeepCopy(), key, pt)).Result;

            Assert.AreEqual(ct.ToHex(), result.ToHex());
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldDecryptCorrectly(string label, BitString iv, BitString pt, BitString key, BitString ct)
        {
            var result = _subjectCs3
                .ProcessPayload(new ModeBlockCipherParameters(BlockCipherDirections.Decrypt, iv.GetDeepCopy(), key, ct)).Result;

            Assert.AreEqual(pt.ToHex(), result.ToHex());
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldEncryptDecryptToSameValue(string label, BitString iv, BitString pt, BitString key, BitString ct)
        {
            var encryptResult = _subjectCs3
                .ProcessPayload(new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, iv.GetDeepCopy(), key, pt)).Result;

            var decryptResult = _subjectCs3
                .ProcessPayload(new ModeBlockCipherParameters(BlockCipherDirections.Decrypt, iv.GetDeepCopy(), key, encryptResult)).Result;

            Assert.AreEqual(pt.GetMostSignificantBits(pt.BitLength).ToHex(), decryptResult.GetMostSignificantBits(pt.BitLength).ToHex());
        }

        [Test]
        public void ShouldEncryptNotModEightCs1()
        {
            var payloadLen = 374;

            var iv = new BitString("00000000000000000000000000000000");
            var key = new BitString("00000000000000000000000000000000");
            var pt = new BitString("F34481EC3CC627BACD5DC3FB08F273E600000000000000000000000000000000000000000000000000000000000000")
                .GetMostSignificantBits(payloadLen);

            var encryptResult = _subjectCs1
                .ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt,
                        iv.GetDeepCopy(),
                        key.GetDeepCopy(),
                        pt.GetDeepCopy())).Result;

            var decryptResult = _subjectCs1
                .ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Decrypt,
                        iv.GetDeepCopy(),
                        key.GetDeepCopy(),
                        encryptResult.GetDeepCopy())).Result;

            Assert.AreEqual(pt.ToHex(), decryptResult.ToHex(), "Expected decrypt back to original PT");
        }

        [Test]
        public void ShouldEncryptNotModEightCs2()
        {
            var payloadLen = 374;

            var iv = new BitString("00000000000000000000000000000000");
            var key = new BitString("00000000000000000000000000000000");
            var pt = new BitString("F34481EC3CC627BACD5DC3FB08F273E600000000000000000000000000000000000000000000000000000000000000")
                .GetMostSignificantBits(payloadLen);

            var encryptResult = _subjectCs2
                .ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt,
                        iv.GetDeepCopy(),
                        key.GetDeepCopy(),
                        pt.GetDeepCopy())).Result;

            var decryptResult = _subjectCs2
                .ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Decrypt,
                        iv.GetDeepCopy(),
                        key.GetDeepCopy(),
                        encryptResult.GetDeepCopy())).Result;

            Assert.AreEqual(pt.ToHex(), decryptResult.ToHex(), "Expected decrypt back to original PT");
        }

        [Test]
        public void ShouldEncryptNotModEightCs3()
        {
            var payloadLen = 374;

            var iv = new BitString("00000000000000000000000000000000");
            var key = new BitString("00000000000000000000000000000000");
            var pt = new BitString("F34481EC3CC627BACD5DC3FB08F273E600000000000000000000000000000000000000000000000000000000000000")
                .GetMostSignificantBits(payloadLen);

            var expectedCtAsPerAegis = new BitString("0336763E966D92595A567CC9CE537F5ED9492AAFC53406FCD852F32A99EED2AD6EF1A3F88079814A8E7083AB12EB10")
                .GetMostSignificantBits(payloadLen);
            var expectedCtFromServer = new BitString("0336763E966D92595A567CC9CE537F5ED2AD6EF1A3F88079814A8E7083AB12EBD9492AAFC53406FCD852F32A99EE08")
                .GetMostSignificantBits(payloadLen);

            var encryptResult = _subjectCs3
                .ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Encrypt,
                        iv.GetDeepCopy(),
                        key.GetDeepCopy(),
                        pt.GetDeepCopy())).Result;

            var decryptResult = _subjectCs3
                .ProcessPayload(
                    new ModeBlockCipherParameters(
                        BlockCipherDirections.Decrypt,
                        iv.GetDeepCopy(),
                        key.GetDeepCopy(),
                        encryptResult.GetDeepCopy())).Result;

            //Assert.AreEqual(expectedCtFromServer.ToHex(), encryptResult.ToHex(), "Expected CT");
            Assert.AreEqual(pt.ToHex(), decryptResult.ToHex(), "Expected decrypt back to original PT");
        }
    }
}
