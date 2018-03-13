using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
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
        [TestCase("message")]
        [TestCase("MESSAGE")]
        [TestCase("msg")]
        [TestCase("MSG")]
        public void ShouldSetMessage(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Message.ToHex());
        }

        [Test]
        [TestCase("Digest")]
        [TestCase("digest")]
        [TestCase("dig")]
        [TestCase("DIG")]
        public void ShouldSetDigest(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Digest.ToHex());
        }
    }
}
