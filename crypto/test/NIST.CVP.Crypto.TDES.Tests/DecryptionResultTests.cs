using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES.Tests
{
    [TestFixture,  FastCryptoTest]
    public class DecryptionResultTests
    {
        [Test]
        public void ShouldSetSuppliedPlainText()
        {
            var plain = new BitString("AE1100");
            var subject= new DecryptionResult(plain);
            Assert.AreEqual(plain, subject.PlainText);
        }

        [Test]
        public void ShouldBeSuccessfulIfPlainSet()
        {
            var plain = new BitString("AE1100");
            var subject = new DecryptionResult(plain);
            Assert.IsTrue((bool) subject.Success);
        }

        [Test]
        public void ShouldSetSuppliedErrorMessage()
        {  
            var subject = new DecryptionResult("ooops!");
            Assert.AreEqual("ooops!", subject.ErrorMessage);
        }

        [Test]
        public void ShouldNotBeSuccessfulIfErrorSet()
        {
            var subject = new DecryptionResult("ooops!");
            Assert.IsFalse((bool) subject.Success);
        }

        [Test]
        public void ShouldShowResultInToStringIfSuccessful()
        {
            var plain = new BitString("AE1100");
            var subject = new DecryptionResult(plain);
            Assume.That((bool) subject.Success);
            Assert.AreEqual("PlainText: AE1100", subject.ToString());
        }

        [Test]
        public void ShouldShowErrorInToStringIfNotSuccessful()
        {

            var subject = new DecryptionResult("ooops!");
            Assume.That((bool) !subject.Success);
            Assert.AreEqual("ooops!", subject.ToString());
        }
    }
}
