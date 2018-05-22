using NIST.CVP.Generation.CMAC;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorVerTests
    {
        private TestCaseValidatorVer<TestGroup, TestCase> _subject;

        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorVer<TestGroup, TestCase>(testCase);
            var result = _subject.Validate(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldFailIfResultDoesNotMatch()
        {
            var testCase = GetTestCase();
            testCase.TestPassed = true;
            _subject = new TestCaseValidatorVer<TestGroup, TestCase>(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.TestPassed = false;
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldThrowErrorWhenReasonNotPresent()
        {
            var testCase = GetTestCase();
            testCase.TestPassed = true;
            _subject = new TestCaseValidatorVer<TestGroup, TestCase>(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.TestPassed = null;
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.IsTrue(Core.Enums.Disposition.Failed == result.Result);
        }
        
        [Test]
        public void ShouldShowResultAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            testCase.TestPassed = true;
            _subject = new TestCaseValidatorVer<TestGroup, TestCase>(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.TestPassed = false;
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.IsTrue(Core.Enums.Disposition.Failed == result.Result);
        }
        
        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                TestPassed = true,
                Message = new BitString(1),
                Mac = new BitString(1),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
