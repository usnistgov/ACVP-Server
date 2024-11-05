using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX963;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX943
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
        public void ShouldReturnFalseIfPassObjectCannotCast()
        {
            var subject = new TestGroup();
            var result = subject.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase("SHARED SECRET LENGTH")]
        [TestCase("shared secret length")]
        public void ShouldSetKeyLength(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "128");
            Assert.That(result, Is.True);
            Assert.That(subject.FieldSize, Is.EqualTo(128));
        }
    }
}
