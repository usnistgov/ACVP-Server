using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XECDH.RFC7748.KeyVer
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

            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public async Task ShouldRunVerifyMethodAndFailWithBadTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
        }

        private TestCase GetResultTestCase(bool shouldPass)
        {
            return new TestCase
            {
                TestCaseId = 1,
                TestPassed = shouldPass   // Says the test Disposition.Passed
            };
        }
    }
}
