using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.TDES_CTR.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorNullTests
    {
        [Test]
        public async Task ShouldReturnErrorForAnyTestCase()
        {
            var testCase = new TestCase();
            var subject = new TestCaseValidatorNull(testCase);
            var result = await subject.ValidateAsync(new TestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.AreEqual("Test type was not found", result.Reason);
        }
    }
}
