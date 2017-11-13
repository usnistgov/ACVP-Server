using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.CMAC.Tests
{
    [TestFixture, FastCryptoTest]
    public class CmacAesHealthTests
    {
        private CmacAes _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new CmacAes(new RijndaelFactory(new RijndaelInternals()));
        }

        [Test]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c", "", "bb1d6929e95937287fa37d129b756746")]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c", "6bc1bee22e409f96e93d7e117393172a", "070a16b46b4d4144f79bdd9dd04a287c")]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c", "6bc1bee22e409f96e93d7e117393172aae2d8a571e03ac9c9eb76fac45af8e5130c81c46a35ce411", "dfa66747de9ae63030ca32611497c827")]
        [TestCase("2b7e151628aed2a6abf7158809cf4f3c", "6bc1bee22e409f96e93d7e117393172aae2d8a571e03ac9c9eb76fac45af8e5130c81c46a35ce411e5fbc1191a0a52eff69f2445df4f9b17ad2b417be66c3710", "51f0bebf7e3b9d92fc49741779363cfe")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b", "", "d17ddf46adaacde531cac483de7a9367")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b", "6bc1bee22e409f96e93d7e117393172a", "9e99a7bf31e710900662f65e617c5184")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b", "6bc1bee22e409f96e93d7e117393172aae2d8a571e03ac9c9eb76fac45af8e5130c81c46a35ce411", "8a1de5be2eb31aad089a82e6ee908b0e")]
        [TestCase("8e73b0f7da0e6452c810f32b809079e562f8ead2522c6b7b", "6bc1bee22e409f96e93d7e117393172aae2d8a571e03ac9c9eb76fac45af8e5130c81c46a35ce411e5fbc1191a0a52eff69f2445df4f9b17ad2b417be66c3710", "a1d5df0eed790f794d77589659f39a11")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4", "", "028962f61b7bf89efc6b551f4667d983")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4", "6bc1bee22e409f96e93d7e117393172a", "28a7023f452e8f82bd4bf28d8c37c35c")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4", "6bc1bee22e409f96e93d7e117393172aae2d8a571e03ac9c9eb76fac45af8e5130c81c46a35ce411", "aaf3d8f1de5640c232f5b169b9c911e6")]
        [TestCase("603deb1015ca71be2b73aef0857d77811f352c073b6108d72d9810a30914dff4", "6bc1bee22e409f96e93d7e117393172aae2d8a571e03ac9c9eb76fac45af8e5130c81c46a35ce411e5fbc1191a0a52eff69f2445df4f9b17ad2b417be66c3710", "e1992190549f6ed5696a2c056c315410")]
        [TestCase("292d770baaaa431c583fe4559027ba6b", "", "eb43ea2ecfa0e601")]
        public void ShouldGenerateAndVerifySuccessfully(string keyString, string messageString, string expectedMacString)
        {
            BitString key = new BitString(keyString);
            BitString message = new BitString(messageString);
            BitString expectedMac = new BitString(expectedMacString);
            int macLength = expectedMac.BitLength;

            Random800_90 rand = new Random800_90();
            var badMac = rand.GetDifferentBitStringOfSameSize(expectedMac);

            var generateResult = _subject.Generate(key, message, macLength);
            var goodValidateResult = _subject.Verify(key, message, generateResult.ResultingMac);
            var badValidateResult = _subject.Verify(key, message, badMac);

            Assert.IsTrue(generateResult.Success, "Successful generate");
            Assert.AreEqual(expectedMac.ToHex(), generateResult.ResultingMac.ToHex(), "Generate MAC");
            Assert.IsTrue(goodValidateResult.Success, "Verify success");
            Assert.IsFalse(badValidateResult.Success, "Verify failure");
        }
    }
}