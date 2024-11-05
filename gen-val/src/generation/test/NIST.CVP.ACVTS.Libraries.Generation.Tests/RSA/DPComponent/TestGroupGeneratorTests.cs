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

            Assert.That(result.Count(), Is.EqualTo(1));

            var testGroup = result.First();
            Assert.That(testGroup.TotalFailingCases, Is.EqualTo(failing), "failing");
            Assert.That(testGroup.TotalTestCases, Is.EqualTo(total), "total");
        }
    }
}
