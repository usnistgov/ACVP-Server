using NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XPN
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
        [TestCase("aad")]
        [TestCase("tag")]
        [TestCase("key")]
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.That(result, Is.True);

        }

        [Test]
        [TestCase("aad")]
        [TestCase("AAD")]
        public void ShouldSetAAD(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.AAD.ToHex(), Is.EqualTo("00AA"));
        }

        [Test]
        [TestCase("key")]
        [TestCase("KEY")]
        public void ShouldSetKey(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.Key.ToHex(), Is.EqualTo("00AA"));
        }

        [Test]
        [TestCase("iv")]
        [TestCase("IV")]
        public void ShouldSetIV(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.IV.ToHex(), Is.EqualTo("00AA"));
        }

        [Test]
        [TestCase("tag")]
        [TestCase("TAG")]
        public void ShouldSetTag(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.Tag.ToHex(), Is.EqualTo("00AA"));
        }

        [Test]
        [TestCase("CipherText")]
        [TestCase("ciphertext")]
        [TestCase("ct")]
        [TestCase("CT")]
        public void ShouldSetCipherText(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.CipherText.ToHex(), Is.EqualTo("00AA"));
        }

        [Test]
        [TestCase("PlainText")]
        [TestCase("PLAINtext")]
        [TestCase("pt")]
        [TestCase("PT")]
        public void ShouldSetPlainText(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.PlainText.ToHex(), Is.EqualTo("00AA"));
        }
    }
}
