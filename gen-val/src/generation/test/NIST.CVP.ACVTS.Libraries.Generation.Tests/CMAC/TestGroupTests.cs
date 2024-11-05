using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
{
    [TestFixture, UnitTest]
    public class TestGroupTests
    {
        [Test]
        [TestCase("Fredo")]
        [TestCase("")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnknownSetStringName(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "1");
            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase("Fredo")]
        [TestCase("A5")]
        [TestCase("NULL")]
        [TestCase(null)]
        public void ShouldReturnFalseIfUnparsableValues(string value)
        {
            var subject = new TestGroup();
            var result = subject.SetString("keyLen", value);
            Assert.That(result, Is.False);
        }

        [Test]
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase("KeyLen")]
        [TestCase("KEYLEN")]
        [TestCase("KLeN")]
        public void ShouldSetKeyLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.That(result, Is.True);
            Assert.That(subject.KeyLength, Is.EqualTo(13));
        }

        [Test]
        [TestCase("msgLen")]
        [TestCase("MSGLEN")]
        [TestCase("MLeN")]
        public void ShouldSetIVLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.That(result, Is.True);
            Assert.That(subject.MessageLength, Is.EqualTo(13));
        }

        [Test]
        [TestCase("macLen")]
        [TestCase("MAcLeN")]
        [TestCase("TLeN")]
        public void ShouldSetTagLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.That(result, Is.True);
            Assert.That(subject.MacLength, Is.EqualTo(13));
        }
    }
}
