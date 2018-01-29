using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA2.Tests
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

            var pubKey = new PublicKey {N = n, E = e};

            var rsa = new Rsa(new RsaVisitor());
            var result = rsa.Encrypt(plainText, pubKey);

            Assert.AreEqual(expectedCipherText, result);
        }

        [Test]
        public void ShouldDecryptWithNormalKeyCorrectly(string pHex, string qHex, string dHex, string nHex, string cipherTextHex, string expectedPlainTextHex)
        {
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var d = new BitString(dHex).ToPositiveBigInteger();
            var n = new BitString(nHex).ToPositiveBigInteger();

            var cipherText = new BitString(cipherTextHex).ToPositiveBigInteger();
            var expectedPlainText = new BitString(expectedPlainTextHex).ToPositiveBigInteger();

            var privKey = new PrivateKey {D = d, P = p, Q = q};
            var pubKey = new PublicKey {N = n};

            var rsa = new Rsa(new RsaVisitor());
            var result = rsa.Decrypt(cipherText, privKey, pubKey);

            Assert.AreEqual(expectedPlainText, result);
        }

        [Test]
        public void ShouldDecryptWithCrtKeyCorrectly(string pHex, string qHex, string dmp1Hex, string dmq1Hex, string iqmpHex, string cipherTextHex, string expectedPlainTextHex)
        {
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var dmp1 = new BitString(dmp1Hex).ToPositiveBigInteger();
            var dmq1 = new BitString(dmq1Hex).ToPositiveBigInteger();
            var iqmp = new BitString(iqmpHex).ToPositiveBigInteger();

            var cipherText = new BitString(cipherTextHex).ToPositiveBigInteger();
            var expectedPlainText = new BitString(expectedPlainTextHex).ToPositiveBigInteger();

            var privKey = new CrtPrivateKey {DMP1 = dmp1, DMQ1 = dmq1, IQMP = iqmp, P = p, Q = q};

            var rsa = new Rsa(new RsaVisitor());
            var result = rsa.Decrypt(cipherText, privKey, null);

            Assert.AreEqual(expectedPlainText, result);
        }
    }
}
