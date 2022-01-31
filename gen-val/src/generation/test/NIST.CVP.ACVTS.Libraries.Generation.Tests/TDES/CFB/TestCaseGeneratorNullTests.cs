using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.v1_0;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFB
{
    [TestFixture]
    public class TestCaseGeneratorNullTests
    {
        [Test]
        public async Task ShouldReturnErrorResponseForIsSampleCall()
        {
            var subject = new TestCaseGeneratorNull();
            var result = await subject.GenerateAsync(new TestGroup(), false);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("This is the null generator -- nothing is generated", result.ErrorMessage);
        }

        [Test]
        public async Task ShouldReturnErrorResponseForTestCaseCall()
        {
            var subject = new TestCaseGeneratorNull();
            var result = await subject.GenerateAsync(new TestGroup(), false);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("This is the null generator -- nothing is generated", result.ErrorMessage);
        }

        [Test]
        public void ShouldHave0NumberOfCases()
        {
            var subject = new TestCaseGeneratorNull();
            Assert.AreEqual(0, subject.NumberOfTestCasesToGenerate);
        }
    }
}
