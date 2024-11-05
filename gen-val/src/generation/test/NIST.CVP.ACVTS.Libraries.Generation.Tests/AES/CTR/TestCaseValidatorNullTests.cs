using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CTR
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

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason, Is.EqualTo("Test type was not found"));
        }
    }
}
