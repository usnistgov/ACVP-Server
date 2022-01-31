using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.DpComponent;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.DPComponent
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        [Test]
        [TestCase(true, 2, 6)]
        [TestCase(false, 10, 30)]
        public async Task ShouldCreateASingleGroupWithCorrectProperties(bool isSample, int failing, int total)
        {
            var parameters = new Parameters
            {
                IsSample = isSample
            };

            var subject = new TestGroupGenerator();
            var result = await subject.BuildTestGroupsAsync(parameters);

            Assert.AreEqual(1, result.Count());

            var testGroup = result.First();
            Assert.AreEqual(failing, testGroup.TotalFailingCases, "failing");
            Assert.AreEqual(total, testGroup.TotalTestCases, "total");
        }
    }
}
