using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Tests
{
    [TestFixture, FastCryptoTest]
    public class RsaTests
    {
        [Test]
        [TestCase("11", "0CA1", "41", "0AE6")]
        [TestCase("424B", "64B1", "485C", "0509")]
        public void ShouldEncryptCorrectly(string eHex, string nHex, string plainTextHex, string expectedCipherTextHex)
        {
            var e = new BitString(eHex).ToPositiveBigInteger();
            var n = new BitString(nHex).ToPositiveBigInteger();

            var plainText = new BitString(plainTextHex).ToPositiveBigInteger();
            var expectedCipherText = new BitString(expectedCipherTextHex).ToPositiveBigInteger();

            var pubKey = new PublicKey { N = n, E = e };

            var rsa = new Rsa(new RsaVisitor());
            var result = rsa.Encrypt(plainText, pubKey);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedCipherText, result.CipherText);
        }

        [Test]
        [TestCase("700746A01C4328DACF3751FBDF4B3D99E2DE5459770F4F1A0BAD919814D80B8DBFB370ECA6893E573D222C4C7A088ED0965DC4546D828A55033850C54646ED524FE28173A1DC51710E3CC5ED00D4B9512EED12C3DDF51F6B18A522C3AE3701DD3BB6B19182D48289B0C31F7A262DA2613402D31C53BD60617A33563725CBF12B",
            "00A80AE9F02A64BD4836D2FAF9CEF0DC66D44D7E863296F6A711845A641F4411549F8D2962F9CDDD82DBB34272B70CD638E18CA67EA443CF7F84D47927E96A63FD17BA5375F8238EACCEFB54F37FB1C8A59D4C6711409B31CA01F1BC214BB5FE7E4F0E4CF36B6B71126C9C8E265827FE1EF21F1D0A0717072646C88591D04B595B",
            "01020304050607",
            "8316321d1bf3dde6c641d22877fdaf1240c6effdae8318628d1e1bb62399096711abe53b0ed592d19a139a193da8e6c8c3b954137b8691bc634cf5183eae342805c028691856d107c3ecac730ca57f84610888b785afcbeee340a589aea6e80c0833a56e72b9ff09af4005f4a5a2072ef67a84fb6101fa6f793bb4426d9a0ca8")]
        [TestCase("700746A01C4328DACF3751FBDF4B3D99E2DE5459770F4F1A0BAD919814D80B8DBFB370ECA6893E573D222C4C7A088ED0965DC4546D828A55033850C54646ED524FE28173A1DC51710E3CC5ED00D4B9512EED12C3DDF51F6B18A522C3AE3701DD3BB6B19182D48289B0C31F7A262DA2613402D31C53BD60617A33563725CBF12B",
            "00A80AE9F02A64BD4836D2FAF9CEF0DC66D44D7E863296F6A711845A641F4411549F8D2962F9CDDD82DBB34272B70CD638E18CA67EA443CF7F84D47927E96A63FD17BA5375F8238EACCEFB54F37FB1C8A59D4C6711409B31CA01F1BC214BB5FE7E4F0E4CF36B6B71126C9C8E265827FE1EF21F1D0A0717072646C88591D04B595B",
            "0102030405060708090A0B0C0D0E0F10",
            "0a702017057ba8c6194127a7a5d580b4253cd8fe72470e1e5266d5acca0b6bb086b880d57749104000b939461bd2e9ad02a7c70e3a04a4e24aa4890182e8c93bd0e6f5ae4ce3853545bbcf663f816ebb990d4d20bda92e38665977ac9c3b30dd505046a2a7b3a7e2e09a85605712b89f17947840d3f2aad8731e791c05a2ec81")]
        public void ShouldDecryptWithNormalKeyCorrectly(string dHex, string nHex, string cipherTextHex, string expectedPlainTextHex)
        {
            var d = new BitString(dHex).ToPositiveBigInteger();
            var n = new BitString(nHex).ToPositiveBigInteger();

            var cipherText = new BitString(cipherTextHex).ToPositiveBigInteger();
            var expectedPlainText = new BitString(expectedPlainTextHex).ToPositiveBigInteger();

            var privKey = new PrivateKey { D = d };
            var pubKey = new PublicKey { N = n };

            var rsa = new Rsa(new RsaVisitor());
            var result = rsa.Decrypt(cipherText, privKey, pubKey);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedPlainText, result.PlainText);
        }

        [Test]
        [TestCase("3D", "35", "35", "31", "26", "0AE6", "41")]
        public void ShouldDecryptWithCrtKeyCorrectly(string pHex, string qHex, string dmp1Hex, string dmq1Hex, string iqmpHex, string cipherTextHex, string expectedPlainTextHex)
        {
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var dmp1 = new BitString(dmp1Hex).ToPositiveBigInteger();
            var dmq1 = new BitString(dmq1Hex).ToPositiveBigInteger();
            var iqmp = new BitString(iqmpHex).ToPositiveBigInteger();

            var cipherText = new BitString(cipherTextHex).ToPositiveBigInteger();
            var expectedPlainText = new BitString(expectedPlainTextHex).ToPositiveBigInteger();

            var privKey = new CrtPrivateKey { DMP1 = dmp1, DMQ1 = dmq1, IQMP = iqmp, P = p, Q = q };
            var pubKey = new PublicKey { N = p * q };

            var rsa = new Rsa(new RsaVisitor());
            var result = rsa.Decrypt(cipherText, privKey, pubKey);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedPlainText, result.PlainText);
        }
    }
}
