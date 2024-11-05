using NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DRBG
{
    [TestFixture, UnitTest]
    public class TestCaseTests
    {
        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase("returnedBits")]
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase("entropyInput")]
        [TestCase("ENTROPYinput")]
        public void ShouldSetEntropy(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.EntropyInput.ToHex(), Is.EqualTo("00AA"));
        }

        [Test]
        [TestCase("nonce")]
        [TestCase("NONCE")]
        public void ShouldSetNonce(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.Nonce.ToHex(), Is.EqualTo("00AA"));
        }

        [Test]
        [TestCase("personalizationString")]
        [TestCase("PERSOnalIzAtiOnstring")]
        [TestCase("PERSOstring")]
        public void ShouldSetPersoString(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.PersoString.ToHex(), Is.EqualTo("00AA"));
        }
    }
}
