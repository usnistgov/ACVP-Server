using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KeyWrap.Tests
{
    [TestFixture,  FastCryptoTest]
    public class KeyWrapTdesTests
    {
        [TestCase("CASE 13: TKW with TDES, wrap 64 bits of plaintext with a 192-bit key", KeyWrapType.TDES_KW, true, true, 
                  "12dfe6e49a9921b61937e9d3e997d047bb753600a830b858", "3105d1045ea36c6a", "a86d49482f1c3f17550749ce")]
        [TestCase("CASE 22: TKW with TDES, wrap 64 bits of plaintext with a 192-bit key", KeyWrapType.TDES_KW, false, true, 
                  "000102030405060708090A0B0C0D0E0F1011121314151617", "0011223344556677", "16277D1DB80D82A76DE53A76")]
        [TestCase("CASE 23: TKW with TDES, wrap 128 bits of plaintext with a 192-bit key", KeyWrapType.TDES_KW, false, true, 
                  "000102030405060708090A0B0C0D0E0F1011121314151617", "00112233445566778899AABBCCDDEEFF", "75F5F26521D739BA33F9619B52D2AB0D29822081")]
        [TestCase("Case 20: TKW_AE, inverse cipher function", KeyWrapType.TDES_KW, true, true,
                  "7565e3977903168a78a2be7b1c81759352b660c4ca46b0f8", "0aaaf5ce132308407885255a9e44f094", "9cc1f8fcf219f401c6f64cebc20a4dfdd557e8c5")]
        [TestCase("Case 21: TKW_AD, inverse cipher function", KeyWrapType.TDES_KW, true, false, 
                  "24a56422f4dca31f78d1ad27259a523d6fc75cb6b4a3cad7", "3ed77e7aa19b73b1ef9c015e", "")]
        public void ShouldReturnExpectedValue(string testLabel, KeyWrapType keyWrapType, bool useInverseCipher, bool successfulAuthenticate, string kString, string pString, string cExpectedString)
        {
            var K = new BitString(kString);
            var P = new BitString(pString);
            var expectedC = new BitString(cExpectedString);

            var subject = new KeyWrapTdes(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
            var actualC = subject.Encrypt(K, P, useInverseCipher);

            // Mangle the actualC returned when it should not decrypt successfully
            if (!successfulAuthenticate)
            {
                var rand = new Random800_90();
                actualC = new SymmetricCipherResult(rand.GetDifferentBitStringOfSameSize(actualC.Result));
            }

            var decrypt = subject.Decrypt(K, actualC.Result, useInverseCipher);

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
