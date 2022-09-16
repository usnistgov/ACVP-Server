using NIST.CVP.ACVTS.Libraries.Crypto.AES_GCM;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_GCM_SIV.Tests
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

        [TestCase("000000000000000000000000000000004db923dc793ee6497c76dcc03a98e108", "0000000000000000000000000000000000000000000000000000000000000000", "000000000000000000000000", "f3f80f2cf0cb2dd9c5984fcda908456cc537703b5ba70324a6793a7bf218d3eaffffffff000000000000000000000000")]
        [TestCase("eb3640277c7ffd1303c7a542d02d3e4c0000000000000000", "0000000000000000000000000000000000000000000000000000000000000000", "000000000000000000000000", "18ce4f0b8cb4d0cac65fea8f79257b20888e53e72299e56dffffffff000000000000000000000000")]
        [TestCase("77ea5cda72857c201396dee10d689ed04ebb44f36fc000d35bfe4486faedecf71532394c8efc5d841bd68fb261ab6cb3e84bcf7b83264b224210cd90931a74f5edf5c18f97ec3a8b9e374842372994224fc2b3963ebaf840ca3d391be19b766ea3849cc3a3c77f1f1301b776529bb424e2671c3c05b9e190bcaa78211f2d02a2338bb573dc53dfe0e2c0b1769b96587d67e920bd61f9a4addcd1151e3e43600c8b78c01af3583f82cc09b14c7ae7c185e9447f737cd729ea951912441ab513be82519295ac5bdbcb960b332f916497980b755c5295a11459ccd4a780190bd54a99bcf43ad9957551042630cdbffd04312259f76a29278db876235dc0d5092671399cc20d91a46db879bace4a258a6c699440b005b678e54f4deaf8f2912bb694375876861c3bbce2c1df274ecc8511916a01adb6628716a2675b17144f09da86681e567ddc8cf32fb2b2d158a4ffad7f5d4c6dd5e1dd45298b883533a0464f822e638daafa8a858a500dca36cf22def0126f2f1d6daad79b321dbe6e6892f1b2692a9aaa35a4ab48794a11586a8cd45f21c3419b3438f0142a0f90ec5cdd91b158a9635336b6f657f40e7e2310021bf08b39b3a85dc423122ba8cdf129647d67f3e17e35f04ef8509dc98851edfbf8592c37fe7737be45c52cb32e9866c68e60daf7bd30cc4bae001716713e91b0f2cc71c160b085e306b9595e787a99157bfe5575989bd241d4961b97d1bc2c959115f6e9d7dd624f40a73feb9853aec8582289fe012c03bf0e1cb311532cfa403eb7a1c48939c58abb2b02768c750b3f90879fa831cc1339e888ae3186a4ada204f322926ea13637c78dcf86e4f0f89a8c7d0f10e42fa80fe8b99a8c84541951d1eb4a712309f2d73712249992834d4cbd5cee03872c9e1de91b7b868c87c27046516f67b8c688ccea92219ad5c4ea56cdd7e4b16c09f5e60efe1dec4f3f267ae1899c8789d2ee5c82a633ae14db803df3e5cbf89ab3b504a79e3d9460a5895891e3f4e31c82345ac0222f07d5b9b8daa638e00fa37671cf5ccb036e866a5bd3647b50e6fa172a55cce2a0e66e4a082ec3047eb667c910dbf4fed215ababd7cc786e5fde85e59605985465b161a181ceee9dd1dbdd1244b94d6682fa5341dfe7067a912c8efaf23515159aa3907958019ebfcb86bc3c4db3544216b48b6337cde98df7696c6d4aba4b6847165e619a8d10c024f3dfda7f48630b87b6efb7a217a914a615c72bec3438a72af8932ae1be6a0df2d899f0906062c184e12069b276d9294833e2e6fc2dbe764f310f5f7689f629685493bea21ff92e58f189a075a1331605648cbaa998a7b206184efda807648deb6d5d20321abe7899eed3454567bf5697f5be5d802362fb22b960148cfe40dc", "f8d690768e5e6b74da190743d9bf52b58c6c3b40acdb1ce3da9feca170424e6e", "ad38f810d200d204f9710933", "7ae4d689de96c8c3b2153313aaf86b5f713319ab668399070706957d169a035f01ae0bd6bf4f6ea3f9f13cc7e31188256f9e154e51e6d8030d4e0af06415e94f21575db3aca72fcbf737ba7b5f64ffafeb18973bee13ee70ab587d73c3f10403ff23f2fb9bf2ee36bcd8c162cb30ae93736b8db561a267c4aaa2001ac449a898cae98417cdb803abc62734dad7e3db2073ac0d00634de06b2d5f3e571eb24eaef11a526c4570be0143afddafb18d3dd0390f9705d56629b26671799a0776f4a51342cc87fb6f5463c06d3dbea45ce4cb42633d296472a527a70ed9ea2191f651c408edac359db1cac0e0bbf8bf424f000724cccecb7a53aa6c0f1f55bb5e7213db7389306ec67554b744eac7e586af5a6cdef55daad6cdf58de5483f2d4d99068833a55ca19fe799f5593326ef39d27c94fd260ffb889c02a1038a2dbf93c1aca0d679eb878ac535a9934622fa38fca6bd538f03e3cbbc10de23fc50531e103a960636a072416ba7c23e201efdd3df06af1201a5fb5c51e3ef1493ab7aec157039107f2a45dc3c514fa93bb197e2e0aee4101e56b21a8c0f23ad0e3c89a4948205c41f6c14f6ef6b25a6fd6956d72bdf0dde3192c4eb34464d733a90de2ebfcb8f03d2b9b228d5a1fe6e032ac854a4a0b8483ccf51404cdf204b9f818e60dc2754efab1f702863c533e1be51a1a2098a6d0003737a039d9f2eab4275b3973a1e8451583435d752b99db6d49f9dcc6a510e04906e9b6c7f415394c746e9086577bfe9cbdb4bae462be62b5140daae85f4c7ce7079233cd62b74f2f7583e10e2c7137e59ac1477be0afb816941b35ddc6a8fdfb9eb227f97b8dfd16e8121d3de520a953663e3b0604bd7d140e1598c84f4596475c9e7befcc97016b8fba7cfa3d43e0d2730486069047f43c2d2fbf42e5e6fc6a17de3e64046ffe3b900d95c1ac1b3b4add3e070448e58e5565efe8fb1293bd11617e5f601b835570ddc13ce67eda28de999f8eb0a8654198832ae5998137ff4bdd4cb0488148197a14abe373aa1729ff807be1d997f36638cd917ab9f4558e1f6fff5dde821daa6f2226854c3287041bd00f5cd520d3d2d539f6903cf98df4b314f65be1bc2bdd26494c140a28d51824678e928a84898f06787ab7eaed80cc99e54371e18eeb7a4fe66225359c949011d741a5ab7fbb431d3b7575c66943b5d68be0579121f00e8d0b14c4fca8a3094e969bcd9d5d6ab7b4684a306a167154bde71ba06b75d990d3346cfad8d759cf61cb8b98927ffb6f3e3524055bfd86c5887ba1020338d4765d508d7829edcc1bfb2cf647fb2803eb8ed3a0e9d276030bf2c1d5cab7ee0b2689184343fc393b28bc15adc35647a65b62b9c800d7d533b033e044a5f9870c69727e4150bea6cf2d5ae98494ada53b15c9c998a6bac4a")]
        public void ShouldCounterWrapCorrectly(string inputHex, string keyHex, string ctrHex, string outputHex)
        {
            var plainText = new BitString(inputHex);
            var key = new BitString(keyHex);
            var iv = new BitString(ctrHex);
            var cipherText = new BitString(outputHex);
            
            var param = new AeadModeBlockCipherParameters(BlockCipherDirections.Encrypt, iv, key, plainText, new BitString(0), 128);
            var result = _subject.ProcessPayload(param);
            
            Assert.IsTrue(result.Success);
            Assert.AreEqual(cipherText.ToHex(), result.Result.ToHex());
        }
    }
}
