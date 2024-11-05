using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
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
        [TestCase("key")]
        [TestCase("msg")]
        [TestCase("mac")]
        public void ShouldSetNullValues(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, null);
            Assert.That(result, Is.True);

        }

        [Test]
        [TestCase("key")]
        [TestCase("KEY")]
        public void ShouldSetKey(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.That(result, Is.True);
            Assert.That(subject.Key.ToHex(), Is.EqualTo(value));
        }

        [Test]
        [TestCase("msg")]
        [TestCase("MsG")]
        public void ShouldSetMessage(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.That(result, Is.True);
            Assert.That(subject.Message.ToHex(), Is.EqualTo(value));
        }

        [Test]
        [TestCase("mac")]
        [TestCase("MaC")]
        public void ShouldSetMac(string name)
        {
            const string value = "00AA";

            var subject = new TestCase();
            var result = subject.SetString(name, value);
            Assert.That(result, Is.True);
            Assert.That(subject.Mac.ToHex(), Is.EqualTo(value));
        }
    }
}
