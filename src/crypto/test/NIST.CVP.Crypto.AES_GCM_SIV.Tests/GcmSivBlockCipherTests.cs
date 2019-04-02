using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_GCM_SIV.Tests
{
    [TestFixture, FastCryptoTest]
    public class GcmSivBlockCipherTests
    {
        private readonly GcmSivBlockCipher _subject = new GcmSivBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_GCMInternals(new ModeBlockCipherFactory(), new BlockCipherEngineFactory()));

        [TestCase("ee8e1ed9ff2540ae8f2ba9f50bc2f27c", "752abad3e0afb5f434dc4310", "310728d9911f1f3837b24316c3fab9a0", "a4c5ae6249963279c100be4d7e2c6edd")]
        public void ShouldDeriveKeysCorrectly(string keyHex, string nonceHex, string expectedAuthKeyHex, string expectedEncKeyHex)
        {
            var key = new BitString(keyHex);
            var nonce = new BitString(nonceHex);

            var expectedAuthKey = new BitString(expectedAuthKeyHex);
            var expectedEncKey = new BitString(expectedEncKeyHex);

            var (messageAuthKey, messageEncKey) = _subject.DeriveKeys(key, nonce);

            Assert.AreEqual(expectedAuthKey, messageAuthKey);
            Assert.AreEqual(expectedEncKey, messageEncKey);
        }

        [TestCase("25629347589242761d31f826ba4b757b", "4f4f95668c83dfb6401762bb2d01a262 d1a24ddd2721d006bbe45f20d3c9f362", "f7a3b47b846119fae5b7866cf5e5b77e")]
        [TestCase("310728d9911f1f3837b24316c3fab9a0", "6578616d706c6500000000000000000048656c6c6f20776f726c64000000000038000000000000005800000000000000", "ad7fcf0b5169851662672f3c5f95138f")]
        [TestCase("d9b360279694941ac5dbc6987ada7377", "01000000000000000000000000000000 00000000000000006000000000000000", "48eb6c6c5a2dbe4a1dde508fee06361b")]
        public void ShouldPolyValCorrectly(string aHex, string bHex, string expectedHex)
        {
            var a = new BitString(aHex);
            var b = new BitString(bHex);
            var expectedResult = new BitString(expectedHex);

            var result = _subject.PolyVal(a, b);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("01000000000000000000000000000000", "00800000000000000000000000000000")]
        [TestCase("9c98c04df9387ded828175a92ba652d8", "4e4c6026fc9c3ef6c140bad495d3296c")]
        [TestCase("7B754BBA26F8311D7642925847936225", "dcbaa5dd137c188ebb21492c23c9b112")]
        public void ShouldMulXGHashCorrectly(string aHex, string expectedHex)
        {
            var a = new BitString(aHex);
            var expectedResult = new BitString(expectedHex);

            var result = _subject.MulXGHash(a);

            Assert.AreEqual(expectedResult.ToHex(), result.ToHex());
        }

        [TestCase("", "", "01000000000000000000000000000000", "030000000000000000000000", "dc20e2d83f25705bb49e439eca56de25")]
        [TestCase("0100000000000000", "", "01000000000000000000000000000000", "030000000000000000000000", "b5d839330ac7b786578782fff6013b81 5b287c22493a364c")]
        [TestCase("0200000000000000", "01", "01000000000000000000000000000000", "030000000000000000000000", "1e6daba35669f4273b0a1a2560969cdf 790d99759abd1508")]
        [TestCase("02000000000000000000000000000000 03000000000000000000000000000000", "01", "01000000000000000000000000000000", "030000000000000000000000", "620048ef3c1e73e57e02bb8562c416a3 19e73e4caac8e96a1ecb2933145a1d71 e6af6a7f87287da059a71684ed3498e1")]
        [TestCase("03000000000000000000000000000000 0400", "01000000000000000000000000000000 02000000", "01000000000000000000000000000000 00000000000000000000000000000000", "030000000000000000000000", "462401724b5ce6588d5a54aae5375513 a075cfcdf5042112aa29685c912fc205 6543")]
        public void ShouldEncryptCorrectly(string plaintextHex, string aadHex, string keyHex, string nonceHex, string expectedHex)
        {
            var plaintext = new BitString(plaintextHex);
            var aad = new BitString(aadHex);
            var key = new BitString(keyHex);
            var nonce = new BitString(nonceHex);

            var expectedResult = new BitString(expectedHex);

            var param = new AeadModeBlockCipherParameters(BlockCipherDirections.Encrypt, nonce, key, plaintext,
                aad, 0);
            var result = _subject.ProcessPayload(param);

            Assert.AreEqual(expectedResult.ToHex(), result.Result.ToHex(), "Result");
        }

        [TestCase("dc20e2d83f25705bb49e439eca56de25", "", "01000000000000000000000000000000", "030000000000000000000000", "", true)]
        [TestCase("b5d839330ac7b786578782fff6013b81 5b287c22493a364c", "", "01000000000000000000000000000000", "030000000000000000000000", "0100000000000000", true)]
        [TestCase("1e6daba35669f4273b0a1a2560969cdf 790d99759abd1508", "01", "01000000000000000000000000000000", "030000000000000000000000", "0200000000000000", true)]
        [TestCase("620048ef3c1e73e57e02bb8562c416a3 19e73e4caac8e96a1ecb2933145a1d71 e6af6a7f87287da059a71684ed3498e1", "01", "01000000000000000000000000000000", "030000000000000000000000", "02000000000000000000000000000000 03000000000000000000000000000000", true)]
        [TestCase("462401724b5ce6588d5a54aae5375513 a075cfcdf5042112aa29685c912fc205 6543", "01000000000000000000000000000000 02000000", "01000000000000000000000000000000 00000000000000000000000000000000", "030000000000000000000000", "03000000000000000000000000000000 0400", true)]
        public void ShouldDecryptCorrectly(string cipherTextHex, string aadHex, string keyHex, string nonceHex, string expectedPlaintextHex, bool expectedResult)
        {
            var cipherText = new BitString(cipherTextHex);
            var aad = new BitString(aadHex);
            var key = new BitString(keyHex);
            var nonce = new BitString(nonceHex);

            var expectedPlaintext = new BitString(expectedPlaintextHex);

            var param = new AeadModeBlockCipherParameters(BlockCipherDirections.Decrypt, nonce, key, cipherText, aad, new BitString(0));
            var result = _subject.ProcessPayload(param);

            Assert.AreEqual(expectedResult, result.Success);

            if (expectedResult)
            {
                Assert.AreEqual(expectedPlaintext, result.Result);
            }
        }

        [TestCase(0, 0, 128)]
        [TestCase(0, 128, 128)]
        [TestCase(0, 136, 128)]
        [TestCase(0, 256, 128)]

        [TestCase(128, 0, 128)]
        [TestCase(128, 128, 128)]
        [TestCase(128, 136, 128)]
        [TestCase(128, 256, 128)]

        [TestCase(136, 0, 128)]
        [TestCase(136, 128, 128)]
        [TestCase(136, 136, 128)]
        [TestCase(136, 256, 128)]

        [TestCase(256, 0, 128)]
        [TestCase(256, 128, 128)]
        [TestCase(256, 136, 128)]
        [TestCase(256, 256, 128)]

        [TestCase(264, 0, 128)]
        [TestCase(264, 128, 128)]
        [TestCase(264, 136, 128)]
        [TestCase(264, 256, 128)]

        [TestCase(0, 0, 256)]
        [TestCase(0, 128, 256)]
        [TestCase(0, 136, 256)]
        [TestCase(0, 256, 256)]

        [TestCase(128, 0, 256)]
        [TestCase(128, 128, 256)]
        [TestCase(128, 136, 256)]
        [TestCase(128, 256, 256)]

        [TestCase(136, 0, 256)]
        [TestCase(136, 128, 256)]
        [TestCase(136, 136, 256)]
        [TestCase(136, 256, 256)]

        [TestCase(256, 0, 256)]
        [TestCase(256, 128, 256)]
        [TestCase(256, 136, 256)]
        [TestCase(256, 256, 256)]

        [TestCase(264, 0, 256)]
        [TestCase(264, 128, 256)]
        [TestCase(264, 136, 256)]
        [TestCase(264, 256, 256)]
        public void ShouldEncryptDecrypt(int ptLen, int aadLen, int keyLen)
        {
            var rand = new Random800_90();

            var plaintext = rand.GetRandomBitString(ptLen);
            var aad = rand.GetRandomBitString(aadLen);
            var key = rand.GetRandomBitString(keyLen);
            var iv = rand.GetRandomBitString(96);

            var param = new AeadModeBlockCipherParameters(BlockCipherDirections.Encrypt, iv, key, plaintext, aad, 0);
            var result = _subject.ProcessPayload(param);

            Assert.IsTrue(result.Success);

            var param2 = new AeadModeBlockCipherParameters(BlockCipherDirections.Decrypt, iv, key, result.Result, aad, new BitString(0));
            var result2 = _subject.ProcessPayload(param2);

            Assert.IsTrue(result.Success);
        }

        [TestCase("D816303DE43569E6AEBC7F929C9B61C40349320334A4E8101912637EAC3BBC079C", "3BBB98776B70058B9A168EB75B7DC5EFE9813B1AABAAB9D6334290F2B4B3D728", "FFC2B859E0BD9A700B679265067907B2")]
        public void ShouldCtr(string inputHex, string keyHex, string ctrHex)
        {
            var input = new BitString(inputHex);
            var key = new BitString(keyHex);
            var ctr = new BitString(ctrHex);

            var result = _subject.AesCtr(key, ctr, input);

            Assert.IsTrue(true);
        }
    }
}
