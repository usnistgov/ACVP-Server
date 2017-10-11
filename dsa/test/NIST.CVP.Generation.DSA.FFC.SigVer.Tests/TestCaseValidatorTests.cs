using NIST.CVP.Generation.DSA.FFC.SigVer.Enums;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
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
        public void ShouldRunVerifyMethodAndSucceedWithGoodTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = subject.Validate(GetResultTestCase(supplied));

            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void ShouldRunVerifyMethodAndFailWithBadTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = subject.Validate(GetResultTestCase(supplied));

            Assert.AreEqual("failed", result.Result);
        }

        private TestCase GetResultTestCase(bool shouldPass)
        {
            return new TestCase
            {
                TestCaseId = 1,
                Result = shouldPass,   // Says the test "passed"
                Reason = new TestCaseExpectationReason(SigFailureReasons.ModifyMessage)     // Only matters in the failure event
            };
        }
    }
}
