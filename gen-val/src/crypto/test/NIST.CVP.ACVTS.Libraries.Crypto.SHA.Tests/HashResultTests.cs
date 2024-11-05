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
            Assert.That(subject.Digest, Is.EqualTo(digest));
        }

        [Test]
        public void ShouldBeSuccessfulIfDigestSet()
        {
            var digest = new BitString("123456");
            var subject = new HashResult(digest);
            Assert.That(subject.Success, Is.True);
        }

        [Test]
        public void ShouldSetSuppliedErrorMessage()
        {
            var errorMessage = "oops!";
            var subject = new HashResult(errorMessage);
            Assert.That(subject.ErrorMessage, Is.EqualTo(errorMessage));
        }

        [Test]
        public void ShouldNotBeSuccessfulIfErrorSet()
        {
            var errorMessage = "oops!";
            var subject = new HashResult(errorMessage);
            Assert.That(subject.Success, Is.False);
        }

        [Test]
        public void ShouldShowResultInToStringIfSuccessful()
        {
            var digest = new BitString("123456");
            var subject = new HashResult(digest);
            Assert.That(subject.Success);
            Assert.That(subject.ToString(), Is.EqualTo("Digest: 123456"));
        }

        [Test]
        public void ShouldShowErrorInToStringIfNotSuccessful()
        {
            var subject = new HashResult("oops!");
            Assert.That(!subject.Success);
            Assert.That(subject.ToString(), Is.EqualTo("oops!"));
        }
    }
}
