using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Text;

namespace NIST.CVP.Crypto.ANSIX942.Tests
{
    [TestFixture, FastCryptoTest]
    public class Ansi942ConcatTests
    {
        private readonly IShaFactory _factory = new ShaFactory();

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 64,
            "5E10 B967 A956 0685 3E52 8F04 262A D18A 4767 C761 1639 7139 1E17 CB05 A216 68D4 CE2B 9F15 1617 4080 42CE 0919 5838 23FD 346D 1751 FBE2 341A F2EE 0461 B62F 100F FAD4 F723 F70C 18B3 8238 ED18 3E93 98C8 CA51 7EE0 CBBE FFF9 C594 71FE 2780 9392 4089 480D BC5A 38E9 A1A9 7D23 0381 0684 7D0D 22EC F85F 49A8 6182 1199 BAFC B0D7 4E6A CFFD 7D14 2765 EBF4 C712 414F E4B6 AB95 7F4C B466 B466 0128 9BB8 2060 4282 7284 2EE2 8F11 3CD1 1F39 431C BFFD 8232 54CE 472E 2105 E49B 3D7F 113B 8250 76E6 2645 8580 7BC4 6454 665F 27C5 E4E1 A4BD 0347 0486 3229 81FD C894 CCA1 E293 0987 C92C 15A3 8BC4 2EB3 8810 E867 C443 2F07 259E C00C DBBB 0FB9 9E17 27C7 06DA 58DD",
            "HMAC Key",
            "95D641F4264588E4E2B6")]
        public void ShouldDeriveKeyCorrectly(ModeValues mode, DigestSizes digestSize, int keyLen, string zzHex, string otherInfoHex, string expectedHex)
        {
            var zz = new BitString(zzHex);
            var otherInfo = new BitString(Encoding.ASCII.GetBytes(otherInfoHex));

            var hashFunction = new HashFunction(mode, digestSize);
            var sha = _factory.GetShaInstance(hashFunction);
            var subject = new AnsiX942Concat(sha);

            var result = subject.DeriveKey(zz, keyLen, otherInfo);

            Assert.True(result.Success);
            Assert.AreEqual(expectedHex, result.DerivedKey.ToHex());
        }
    }
}