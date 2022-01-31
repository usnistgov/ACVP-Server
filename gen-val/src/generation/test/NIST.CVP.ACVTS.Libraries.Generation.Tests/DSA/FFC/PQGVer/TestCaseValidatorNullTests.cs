using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.PQGVer
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorNullTests
    {
        [Test]
        public async Task ShouldReturnFailedForInitialValidate()
        {
            var subject = new TestCaseValidatorNull("error", 0);
            var result = await subject.ValidateAsync(new TestCase());
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }
    }
}
