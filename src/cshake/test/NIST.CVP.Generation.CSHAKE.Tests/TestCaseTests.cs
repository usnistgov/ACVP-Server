using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CSHAKE.Tests
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

        // Note: These hex strings are little endian
        [Test]
        [TestCase("01", 2)]
        [TestCase("AB0C", 12)]
        [TestCase("AFF56703", 25)]
        public void ShouldSetMessageWithLengthAsLittleEndian(string hex, int length)
        {
            var expectedResult = new BitString(hex, length, false);
            var subject = new TestCase();
            var result = subject.SetString("msg", hex, length);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedResult, subject.Message);
        }

        [Test]
        [TestCase("Digest")]
        [TestCase("digest")]
        [TestCase("dig")]
        [TestCase("DIG")]
        [TestCase("md")]
        public void ShouldSetDigest(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.IsTrue(result);
            Assert.AreEqual("00AA", subject.Digest.ToHex());
        }

        // Note: These hex strings are little endian
        [Test]
        [TestCase("01", 2)]
        [TestCase("AB0C", 12)]
        [TestCase("AFF56703", 25)]
        public void ShouldSetDigestWithLengthAsLittleEndian(string hex, int length)
        {
            var expectedResult = new BitString(hex, length, false);
            var subject = new TestCase();
            var result = subject.SetString("md", hex, length);
            Assert.IsTrue(result);
            Assert.AreEqual(expectedResult, subject.Digest);
        }
    }
}
