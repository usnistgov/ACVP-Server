using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES_OFB.Tests
{
    [TestFixture, FastCryptoTest]
    public class TdesOfbTests
    {

        [Test]
        public void ShouldEncrypt()
        {
            var key = new BitString("0101010101010101");
            var plainText = new BitString("0000000000000000");
            var expectedCipherText = new BitString("95f8a5e5dd31d900");
            var iv = new BitString("8000000000000000");
            var subject = new TdesOfb();
            var result = subject.BlockEncrypt(key, plainText, iv);

            Assert.IsTrue((bool)result.Success);
            Assert.AreEqual(expectedCipherText.ToHex(), result.CipherText.ToHex());
        }

        [Test]
        public void ShouldDecrypt()
        {
            var key = new BitString("0101010101010101");
            var expectedPlainText = new BitString("0000000000000000");
            var cipherText = new BitString("95f8a5e5dd31d900");
            var iv = new BitString("8000000000000000");
            var subject = new TdesOfb();
            var result = subject.BlockDecrypt(key, cipherText, iv);

            Assert.IsTrue((bool)result.Success);
            Assert.AreEqual(expectedPlainText.ToHex(), result.PlainText.ToHex());
        }

        [Test]
        public void ShouldFailEncrypt()
        {
            var keyBytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            var PlainTextBytes = new byte[] { 0x3f, 0xa4, 0x0e, 0x8a };
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var subject = new TdesOfb();
            var result = subject.BlockEncrypt(new BitString(keyBytes), new BitString(PlainTextBytes), new BitString(iv));
            Assert.IsFalse((bool)result.Success);
            Assert.IsNotNull(result.ErrorMessage);
        }

        public void ShouldFailDecrypt()
        {
            var keyBytes = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef };
            var CipherTextBytes = new byte[] { 0x3f, 0xa4, 0x0e, 0x8a };
            var iv = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var subject = new TdesOfb();
            var result = subject.BlockDecrypt(new BitString(keyBytes), new BitString(CipherTextBytes), new BitString(iv));
            Assert.IsFalse((bool)result.Success);
            Assert.IsNotNull(result.ErrorMessage);
        }


    }
}
