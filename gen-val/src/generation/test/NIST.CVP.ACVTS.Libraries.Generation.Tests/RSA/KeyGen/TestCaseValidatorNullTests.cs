using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.KeyGen
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
