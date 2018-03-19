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
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch(bool testPassed)
        {
            var testCase = GetTestCase(testPassed);
            var subject = new TestCaseValidatorKat(testCase);

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldNotValidateIfExpectedAndSuppliedDoNotMatch(bool testPassed)
        {
            var testCase = GetTestCase(testPassed);
            var subject = new TestCaseValidatorKat(GetTestCase(!testPassed));

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetTestCase(bool testPassed)
        {
            return new TestCase
            {
                TestCaseId = 1,
                TestPassed = testPassed
            };
        }
    }
}
