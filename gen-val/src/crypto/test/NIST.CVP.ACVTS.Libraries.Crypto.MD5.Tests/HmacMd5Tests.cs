using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.MD5.Tests
{
    [TestFixture, FastCryptoTest]
    public class HmacMd5Tests
    {
        [Test]
        [TestCase("", "", "74e6f7298a9c2d168935f58c001bad88")]
        [TestCase("The quick brown fox jumps over the lazy dog", "key", "80070713463e7749b90c2dc24911e275")]
        public void ShouldHmacMd5ASCIITextCorrectly(string text, string keyHex, string expectedDigestHex)
        {
            var message = new BitString(Encoding.ASCII.GetBytes(text));
            var key = new BitString(Encoding.ASCII.GetBytes(keyHex));
            var expectedDigest = new BitString(expectedDigestHex);

            var subject = new HmacMd5(new Md5());

            var result = subject.Generate(key, message);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedDigest.ToHex(), result.Mac.ToHex());
        }
    }
}
