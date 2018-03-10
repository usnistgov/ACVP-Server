using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XPN.Tests
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
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("aad")]
        [TestCase("tag")]
        [TestCase("key")]
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.IsTrue(result);
            
        }

        [Test]
        [TestCase("aad")]
        [TestCase("AAD")]
        public void ShouldSetAAD(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.AAD.ToHex());
        }

        [Test]
        [TestCase("key")]
        [TestCase("KEY")]
        public void ShouldSetKey(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Key.ToHex());
        }

        [Test]
        [TestCase("iv")]
        [TestCase("IV")]
        public void ShouldSetIV(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.IV.ToHex());
        }

        [Test]
        [TestCase("tag")]
        [TestCase("TAG")]
        public void ShouldSetTag(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Tag.ToHex());
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
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.CipherText.ToHex());
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
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.PlainText.ToHex());
        }
    }
}
