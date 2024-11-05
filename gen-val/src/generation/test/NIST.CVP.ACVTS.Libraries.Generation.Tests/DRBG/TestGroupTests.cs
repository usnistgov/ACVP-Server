using NIST.CVP.ACVTS.Libraries.Generation.DRBG.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DRBG
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
            var result = subject.SetString("entropyinputlen", value);
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
        [TestCase("entropyinputlen")]
        [TestCase("ENtropYInpuTLen")]
        public void ShouldSetEntropyLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "13");
            Assert.That(result, Is.True);
            Assert.That(subject.EntropyInputLen, Is.EqualTo(13));
        }
    }
}
