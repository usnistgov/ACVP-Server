using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.PQGVer
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public async Task ShouldRunVerifyMethodAndFailWithBadTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        private TestCase GetResultTestCase(bool shouldPass)
        {
            return new TestCase
            {
                TestCaseId = 1,
                P = BitString.To32BitString(2),
                Q = BitString.To32BitString(3),
                TestPassed = shouldPass   // Says the test Core.Enums.Disposition.Passed
            };
        }
    }
}
