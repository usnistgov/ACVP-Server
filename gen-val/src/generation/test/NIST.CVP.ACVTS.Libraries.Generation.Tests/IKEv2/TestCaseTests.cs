using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.IKEv2
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
        [TestCase("nResp")]
        [TestCase("nr")]
        [TestCase("NR")]
        public void ShouldSetNResp(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.NResp.ToHex(), Is.EqualTo("00AA"));
        }

        [Test]
        [TestCase("nInit")]
        [TestCase("ni")]
        [TestCase("NI")]
        public void ShouldSetNInit(string name)
        {
            var subject = new TestCase();
            var result = subject.SetString(name, "00AA");
            Assert.That(result, Is.True);
            Assert.That(subject.NInit.ToHex(), Is.EqualTo("00AA"));
        }
    }
}
