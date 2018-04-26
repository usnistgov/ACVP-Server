using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.Tests
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
            var result = subject.SetString("ivlen", value);
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
        public void ShouldSetKeyLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name,"13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.KeyLength);
        }

        [Test]
        [TestCase("IvLen")]
        [TestCase("IvLEN")]
        public void ShouldSetIVLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.IVLength);
        }

        [Test]
        [TestCase("tagLen")]
        [TestCase("TaGLEN")]
        public void ShouldSetTagLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.TagLength);
        }

        [Test]
        [TestCase("aadLen")]
        [TestCase("AADLEN")]
        public void ShouldSetAADLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.AADLength);
        }

        [Test]
        [TestCase("ptLen")]
        [TestCase("ptlen")]
        public void ShouldSetPTLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.IsTrue(result);
            Assert.AreEqual(13, subject.PTLength);
        }
    }
}
