using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorKatTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch(bool failureTest)
        {
            var testCase = GetTestCase(failureTest);
            var subject = new TestCaseValidatorKat(testCase);

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldNotValidateIfExpectedAndSuppliedDoNotMatch(bool failureTest)
        {
            var testCase = GetTestCase(failureTest);
            var subject = new TestCaseValidatorKat(GetTestCase(!failureTest));

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetTestCase(bool failureTest)
        {
            return new TestCase
            {
                TestCaseId = 1,
                FailureTest = failureTest
            };
        }
    }
}
