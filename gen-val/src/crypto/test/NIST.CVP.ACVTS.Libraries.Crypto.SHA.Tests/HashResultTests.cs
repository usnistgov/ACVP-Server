using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA.Tests
{
    [TestFixture]
    [FastCryptoTest]
    public class HashResultTests
    {
        [Test]
        public void ShouldSetSuppliedDigest()
        {
            var digest = new BitString("123456");
            var subject = new HashResult(digest);
            Assert.AreEqual(digest, subject.Digest);
        }

        [Test]
        public void ShouldBeSuccessfulIfDigestSet()
        {
            var digest = new BitString("123456");
            var subject = new HashResult(digest);
            Assert.IsTrue(subject.Success);
        }

        [Test]
        public void ShouldSetSuppliedErrorMessage()
        {
            var errorMessage = "oops!";
            var subject = new HashResult(errorMessage);
            Assert.AreEqual(errorMessage, subject.ErrorMessage);
        }

        [Test]
        public void ShouldNotBeSuccessfulIfErrorSet()
        {
            var errorMessage = "oops!";
            var subject = new HashResult(errorMessage);
            Assert.IsFalse(subject.Success);
        }

        [Test]
        public void ShouldShowResultInToStringIfSuccessful()
        {
            var digest = new BitString("123456");
            var subject = new HashResult(digest);
            Assert.That(subject.Success);
            Assert.AreEqual("Digest: 123456", subject.ToString());
        }

        [Test]
        public void ShouldShowErrorInToStringIfNotSuccessful()
        {
            var subject = new HashResult("oops!");
            Assert.That(!subject.Success);
            Assert.AreEqual("oops!", subject.ToString());
        }
    }
}
