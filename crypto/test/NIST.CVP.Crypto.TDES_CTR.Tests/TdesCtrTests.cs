using NIST.CVP.Math;
using NUnit.Framework;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Crypto.TDES_CTR.Tests
{
    [TestFixture, FastCryptoTest]
    public class TdesCtrTests
    {
        [Test]
        [TestCase("0101010101010101",
            "8000000000000000",
            "0000000000000000",
            "95f8a5e5dd31d900",
            TestName = "TDES_CTR - Encrypt")]
        public void ShouldEncryptCorrectly(string keyHex, string ivHex, string ptHex, string ctHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex);
            var ct = new BitString(ctHex);

            var subject = new TdesCtr();

            var result = subject.EncryptBlock(key, pt, iv);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(ct, result.Result);
        }

        [Test]
        [TestCase("0101010101010101",
            "8000000000000000",
            "000000000000",
            "95f8a5e5dd20",
            43,
            TestName = "TDES_CTR - Encrypt Partial")]
        public void ShouldEncryptPartialBlockCorrectly(string keyHex, string ivHex, string ptHex, string ctHex,
            int length)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var pt = new BitString(ptHex, length);
            var ct = new BitString(ctHex, length);

            var subject = new TdesCtr();

            var result = subject.EncryptBlock(key, pt, iv);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(ct, result.Result);
        }

        [Test]
        [TestCase("0101010101010101",
            "8000000000000000",
            "95f8a5e5dd31d900",
            "0000000000000000",
            TestName = "TDES_CTR - Decrypt")]
        public void ShouldDecryptCorrectly(string keyHex, string ivHex, string ctHex, string ptHex)
        {
            var key = new BitString(keyHex);
            var iv = new BitString(ivHex);
            var ct = new BitString(ctHex);
            var pt = new BitString(ptHex);

            var subject = new TdesCtr();

            var result = subject.DecryptBlock(key, ct, iv);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(pt, result.Result);
        }
    }
}
