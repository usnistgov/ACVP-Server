using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.CMAC.Tests
{
    [TestFixture, FastCryptoTest]
    public class CmacTdesHealthTests
    {
        private CmacTdes _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new CmacTdes(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
        }
       
        [Test]
        [TestCase("4a4937e5e670139bfbf298f80e925eef73f4e36eba40df23", "ddfa8cc929f345f0fbe960e87a75e1307a28e700a420ea80d0d84137b644e7c6c61e7ed6547b944ff8a126ca351c624a580fb126a3c9f54aab980fa0ffe2a5c8", "4c5b8c33d81b5114")]
        [TestCase("46ad6ebad9644a67da684aa48f23d61943a2316b40a46e25", "cf97c2abe3d0fc89e05538b50147a3f405391219", "7ac08967edc5730b")]
        [TestCase("91c8851934cdecc2582562aef1205e32a12a70eacbad310e", "aa390a0ae33751b0bd8de5723df91d999aa70358", "67f76912ed61eaab")]
        [TestCase("20efc2fed0b03e85048cfd0b707fc8025e52917a07f12c80", "", "5861bc3dc5a3c413")]
        [TestCase("38c783da15c40be61957d334379d58d38f9826e61338fd80", "fed6", "2257f77bcc164745")]
        [TestCase("cefbfd4f86c40e154ab370bfec13b6f225b36145fe0dda80", "bd4fc50fa149283290b772df284cd493c882379b4f44267227eae84a836bece9e4ce6bb6e2e24814292f3beec22c2e9394b3e7c52ab1e80cb6229351563d92a797eb766c387e9e9130a05b36f189517aef7621915c56451736c63ba3843fe5a0dd8e30582985f452acf11bde40cc06eee1b32c276f9a0438b0be266b95dbad", "3d9941c13135ef21")]

        public void ShouldGenerateAndVerifySuccessfully(string keyString, string messageString, string expectedMacString)
        {
            BitString key = new BitString(keyString);
            BitString message = new BitString(messageString);
            BitString expectedMac = new BitString(expectedMacString);
            int macLength = expectedMac.BitLength;

            Random800_90 rand = new Random800_90();
            var badMac = rand.GetDifferentBitStringOfSameSize(expectedMac);

            var generateResult = _subject.Generate(key, message, macLength);
            var goodValidateResult = _subject.Verify(key, message, generateResult.Mac);
            var badValidateResult = _subject.Verify(key, message, badMac);

            Assert.IsTrue(generateResult.Success, "Successful generate");
            Assert.AreEqual(expectedMac.ToHex(), generateResult.Mac.ToHex(), "Generate MAC");
            Assert.IsTrue(goodValidateResult.Success, "Verify success");
            Assert.IsFalse(badValidateResult.Success, "Verify failure");
        }
    }
}
