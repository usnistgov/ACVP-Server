using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SNMP;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SNMP
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
        [TestCase("engineid")]
        [TestCase("engineID")]
        public void ShouldSetEngineId(string name)
        {
            var subject = new TestGroup();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.EngineId.ToHex(), Is.EqualTo("00AA"));
        }
    }
}
