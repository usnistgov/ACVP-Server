using System;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorValTests
    {
        private readonly TestDataMother _tdm = new TestDataMother();
        private TestCaseValidatorVal _subject;

        [Test]
        [TestCase(true, "fail")]
        [TestCase(false, "pass")]
        public void ShouldContainNoErrorsOnValidValidation(bool failureTest, string result)
        {
            var testGroup = _tdm.GetTestGroups()[0];
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.FailureTest = failureTest;
            testCase.Result = result;

            _subject = new TestCaseValidatorVal(testCase);

            var validateResult = _subject.Validate(testCase);

            Assert.IsTrue(validateResult.Result == Core.Enums.Disposition.Passed);
        }

        [Test]
        public void ShouldContainErrorWhenResultNotPresent()
        {
            var testGroup = _tdm.GetTestGroups()[0];
            var testCase = (TestCase)testGroup.Tests[0];
            testCase.Result = null;

            var suppliedTestCase = testCase;

            _subject = new TestCaseValidatorVal(testCase);

            var validateResult = _subject.Validate(suppliedTestCase);

            Assert.IsTrue(validateResult.Result == Core.Enums.Disposition.Failed);
        }
    }
}