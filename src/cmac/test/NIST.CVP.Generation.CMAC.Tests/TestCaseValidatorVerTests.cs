using System.Threading.Tasks;
using NIST.CVP.Generation.CMAC;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorVerTests
    {
        private TestCaseValidatorVer _subject;

        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorVer(testCase);
            var result = await _subject.ValidateAsync(testCase);
            Assume.That((bool) (result != null));
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfResultDoesNotMatch()
        {
            var testCase = GetTestCase();
            testCase.TestPassed = true;
            _subject = new TestCaseValidatorVer(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.TestPassed = false;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That((bool) (result != null));
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldThrowErrorWhenReasonNotPresent()
        {
            var testCase = GetTestCase();
            testCase.TestPassed = true;
            _subject = new TestCaseValidatorVer(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.TestPassed = null;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That((bool) (result != null));
            Assert.IsTrue(Core.Enums.Disposition.Failed == result.Result);
        }
        
        [Test]
        public async Task ShouldShowResultAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            testCase.TestPassed = true;
            _subject = new TestCaseValidatorVer(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.TestPassed = false;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That((bool) (result != null));
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
