using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorValTests
    {
        private TestCaseValidatorVal _subject;

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldContainNoErrorsOnValidValidation(bool testPassed)
        {
            var testGroup = TestDataMother.GetTestGroups(1, false, "val").TestGroups[0];
            var testCase = testGroup.Tests[0];

            testCase.TestPassed = testPassed;

            _subject = new TestCaseValidatorVal(testCase);

            var validateResult = _subject.Validate(testCase);

            Assert.IsTrue(validateResult.Result == Core.Enums.Disposition.Passed);
        }

        [Test]
        public void ShouldContainErrorWhenResultNotPresent()
        {
            var testGroup = TestDataMother.GetTestGroups(1, false, "val").TestGroups[0];
            var testCase = testGroup.Tests[0];
            testCase.TestPassed = null;

            var suppliedTestCase = testCase;

            _subject = new TestCaseValidatorVal(testCase);

            var validateResult = _subject.Validate(suppliedTestCase);

            Assert.IsTrue(validateResult.Result == Core.Enums.Disposition.Failed);
        }
    }
}