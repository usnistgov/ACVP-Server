using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_ECB.v1_0;

namespace NIST.CVP.Generation.AES_ECB.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorNullTests
    {
        [Test]
        public void ShouldHaveZeroForNumberOfTestCases()
        {
            var subject = new TestCaseGeneratorNull();
            Assert.AreEqual(1, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public async Task ShouldReturnErrorForInitialGenerate()
        {
            var subject = new TestCaseGeneratorNull();
            var result = await subject.GenerateAsync(new TestGroup(), false);
            Assert.IsFalse(result.Success);
        }
    }
}
