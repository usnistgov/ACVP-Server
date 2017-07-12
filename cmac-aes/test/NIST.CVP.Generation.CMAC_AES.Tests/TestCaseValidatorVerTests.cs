using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorVerTests
    {
        private TestCaseValidatorVer _subject;

        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorVer(testCase);
            var result = _subject.Validate(testCase);
            Assume.That(result != null);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldFailIfResultDoesNotMatch()
        {
            var testCase = GetTestCase();
            testCase.Result = "pass";
            _subject = new TestCaseValidatorVer(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Result = "fail";
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual("failed", result.Result);
        }

        [Test]
        public void ShouldShowPlainTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            testCase.Result = "pass";
            _subject = new TestCaseValidatorVer(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Result = "fail";
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);
            Assert.IsTrue(result.Reason.Contains("Result"));
        }
        
        private TestCase GetTestCase(bool failureTest = false)
        {
            var testCase = new TestCase
            {
                FailureTest = failureTest,
                Message = new BitString(1),
                Mac = new BitString(1),
                Result = "test",
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
