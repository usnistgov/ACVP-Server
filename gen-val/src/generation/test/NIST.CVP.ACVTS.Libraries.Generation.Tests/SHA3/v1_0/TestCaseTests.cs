using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA3.v1_0
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
        [TestCase("message")]
        [TestCase("MESSAGE")]
        [TestCase("msg")]
        [TestCase("MSG")]
        public void ShouldSetMessage(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.Message.ToHex(), Is.EqualTo("00AA"));
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
            Assert.That(result, Is.True);
            Assert.That(subject.Message, Is.EqualTo(expectedResult));
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
            Assert.That(result, Is.True);
            Assert.That(subject.Digest.ToHex(), Is.EqualTo("00AA"));
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
            Assert.That(result, Is.True);
            Assert.That(subject.Digest, Is.EqualTo(expectedResult));
        }
    }
}
