using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.MD5.Tests
{
    [TestFixture, FastCryptoTest]
    public class Md5Tests
    {
        [Test]
        [TestCase("", "d41d8cd98f00b204e9800998ecf8427e")]
        [TestCase("36363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636", "c5738ffbaa1ff9d62e688841e89e608e")]
        [TestCase("5d534f3636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363636363654686520717569636b2062726f776e20666f78206a756d7073206f76657220746865206c617a7920646f67", "1deaf7c64f3a0383fb9e1b932ba5749d")]
        public void ShouldMd5Correctly(string messageHex, string expectedDigestHex)
        {
            var message = new BitString(messageHex);
            var expectedDigest = new BitString(expectedDigestHex);

            var subject = new Md5();

            var result = subject.Hash(message);

            Assert.That(result.Success, Is.True, result.ErrorMessage);
            Assert.That(result.Digest.ToHex(), Is.EqualTo(expectedDigest.ToHex()));
        }

        [Test]
        [TestCase("a", "0CC175B9C0F1B6A831C399E269772661")]
        [TestCase("abc", "900150983CD24FB0D6963F7D28E17F72")]
        [TestCase("The quick brown fox jumps over the lazy dog", "9e107d9d372bb6826bd81d3542a419d6")]
        [TestCase("Hello World!", "ED076287532E86365E841E92BFC50D8C")]
        public void ShouldMd5ASCIITextCorrectly(string text, string expectedDigestHex)
        {
            var message = new BitString(Encoding.ASCII.GetBytes(text));
            var expectedDigest = new BitString(expectedDigestHex);

            var subject = new Md5();

            var result = subject.Hash(message);

            Assert.That(result.Success, Is.True, result.ErrorMessage);
            Assert.That(result.Digest.ToHex(), Is.EqualTo(expectedDigest.ToHex()));
        }
    }
}
