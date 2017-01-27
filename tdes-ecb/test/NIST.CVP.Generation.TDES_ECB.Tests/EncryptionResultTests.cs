using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
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
            Assert.IsTrue(subject.Success);
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
            Assert.IsFalse(subject.Success);
        }

        [Test]
        public void ShouldShowResultInToStringIfSuccessful()
        {
            var cipher = new BitString("AE1100");
            var subject = new EncryptionResult(cipher);
            Assume.That(subject.Success);
            Assert.AreEqual("CipherText: AE1100", subject.ToString());
        }

        [Test]
        public void ShouldShowErrorInToStringIfNotSuccessful()
        {
            
            var subject = new EncryptionResult("ooops!");
            Assume.That(!subject.Success);
            Assert.AreEqual("ooops!", subject.ToString());
        }
    }
}
