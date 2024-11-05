using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
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
            Assert.That((bool)(result != null));
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
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
            Assert.That((bool)(result != null));
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
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
            Assert.That((bool)(result != null));
            Assert.That(Core.Enums.Disposition.Failed == result.Result, Is.True);
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
            Assert.That((bool)(result != null));
            Assert.That(Core.Enums.Disposition.Failed == result.Result, Is.True);
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
