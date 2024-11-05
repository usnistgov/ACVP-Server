using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.FFC;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.FFC
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorValTests
    {
        private TestCaseValidatorVal _subject;

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldContainNoErrorsOnValidValidation(bool testPassed)
        {
            var testGroup = TestDataMother.GetTestGroups(1, true, "val").TestGroups[0];
            var testCase = testGroup.Tests[0];

            testCase.TestPassed = testPassed;

            _subject = new TestCaseValidatorVal(testCase);

            var validateResult = await _subject.ValidateAsync(testCase);

            Assert.That(validateResult.Result == Core.Enums.Disposition.Passed, Is.True);
        }

        [Test]
        public async Task ShouldContainErrorWhenResultNotPresent()
        {
            var testGroup = TestDataMother.GetTestGroups(1, true, "val").TestGroups[0];
            var testCase = testGroup.Tests[0];
            testCase.TestPassed = null;

            var suppliedTestCase = testCase;

            _subject = new TestCaseValidatorVal(testCase);

            var validateResult = await _subject.ValidateAsync(suppliedTestCase);

            Assert.That(validateResult.Result == Core.Enums.Disposition.Failed, Is.True);
        }
    }
}
