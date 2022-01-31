using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.HMAC
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
            Assert.IsFalse(result);
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
            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("KeyLen")]
        [TestCase("KEYLEN")]
        [TestCase("KLeN")]
        public void ShouldSetKeyLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13 * 8, subject.KeyLength);
        }

        [Test]
        [TestCase("msgLen")]
        [TestCase("MSGLEN")]
        [TestCase("MLeN")]
        public void ShouldSetMsgLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13 * 8, subject.MessageLength);
        }

        [Test]
        [TestCase("macLen")]
        [TestCase("MAcLeN")]
        [TestCase("TLeN")]
        public void ShouldSetMacLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13 * 8, subject.MacLength);
        }
    }
}
