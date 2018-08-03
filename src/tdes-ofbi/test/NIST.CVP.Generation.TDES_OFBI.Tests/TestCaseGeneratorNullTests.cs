using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_OFBI.Tests
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
        public void ShouldHave0NumberOfCases()
        {
            var subject = new TestCaseGeneratorNull();
            Assert.AreEqual(0, subject.NumberOfTestCasesToGenerate);
        }
    }
}
