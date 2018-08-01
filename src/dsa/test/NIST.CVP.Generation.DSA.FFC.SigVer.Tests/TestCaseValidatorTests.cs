using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Generation.DSA.FFC.SigVer.TestCaseExpectations;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.Tests
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

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public async Task ShouldRunVerifyMethodAndFailWithBadTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = await subject.ValidateAsync(GetResultTestCase(supplied));

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetResultTestCase(bool shouldPass)
        {
            return new TestCase
            {
                TestCaseId = 1,
                TestPassed = shouldPass,   // Says the test Core.Enums.Disposition.Passed
                Reason = new TestCaseExpectationReason(DsaSignatureDisposition.ModifyMessage)     // Only matters in the failure event
            };
        }
    }
}
