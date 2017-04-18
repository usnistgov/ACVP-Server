using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TDES.Tests
{
    [TestFixture]
    public class EncryptionResultTests
    {
        [Test]
        public void ShouldSetSuppliedCipherText()
        {
            var cipher = new BitString("AE1100");
            var subject= new EncryptionResult(cipher);
            Assert.AreEqual(cipher, subject.CipherText);
        }

        [Test]
        public void ShouldBeSuccessfulIfCipherSet()
        {
            var cipher = new BitString("AE1100");
            var subject = new EncryptionResult(cipher);
            Assert.IsTrue((bool) subject.Success);
        }

        [Test]
        public void ShouldSetSuppliedErrorMessage()
        {  
            var subject = new EncryptionResult("ooops!");
            Assert.AreEqual("ooops!", subject.ErrorMessage);
        }

        [Test]
        public void ShouldNotBeSuccessfulIfErrorSet()
        {
            var subject = new EncryptionResult("ooops!");
            Assert.IsFalse((bool) subject.Success);
        }

        [Test]
        public void ShouldShowResultInToStringIfSuccessful()
        {
            var cipher = new BitString("AE1100");
            var subject = new EncryptionResult(cipher);
            Assume.That((bool) subject.Success);
            Assert.AreEqual("CipherText: AE1100", subject.ToString());
        }

        [Test]
        public void ShouldShowErrorInToStringIfNotSuccessful()
        {
            
            var subject = new EncryptionResult("ooops!");
            Assume.That((bool) !subject.Success);
            Assert.AreEqual("ooops!", subject.ToString());
        }
    }
}
