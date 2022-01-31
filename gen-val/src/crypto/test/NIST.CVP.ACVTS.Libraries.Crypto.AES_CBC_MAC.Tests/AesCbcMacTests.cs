using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_CBC_MAC.Tests
{
    [TestFixture, FastCryptoTest]
    public class AesCbcMacTests
    {
        [Test]
        // Test vectors for this don't actually exist. These tests were self-generated
        [TestCase("000102030405060708090A0B0C0D0E0F", "0123456789ABCDEF0123456789ABCDEF", "3071708FFD2412229B2677BA5F1C52D2")]
        [TestCase("000102030405060708090A0B0C0D0E0F0001020304050607", "0123456789ABCDEF0123456789ABCDEF", "8880FA5202C71C3743EDB22D208ED385")]
        [TestCase("000102030405060708090A0B0C0D0E0F000102030405060708090A0B0C0D0E0F", "0123456789ABCDEF0123456789ABCDEF", "F243BF16FDE2DCF9F6D3DD8534F86C7E")]
        public void ShouldEncryptCorrectly(string keyHex, string ptHex, string ctHex)
        {
            var key = new BitString(keyHex);
            var payload = new BitString(ptHex);
            var expectedResult = new BitString(ctHex);

            var factory = new BlockCipherEngineFactory();
            var engine = factory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes);

            var subject = new ModeBlockCipherFactory().GetStandardCipher(engine, BlockCipherModesOfOperation.CbcMac);

            var param = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, BitString.Zeroes(128), key, payload);
            var result = subject.ProcessPayload(param);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedResult.ToHex(), result.Result.ToHex());
        }
    }
}
