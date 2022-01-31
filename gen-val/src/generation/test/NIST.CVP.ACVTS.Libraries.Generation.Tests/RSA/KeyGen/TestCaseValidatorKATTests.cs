using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.KeyGen
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorKatTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch(bool testPassed)
        {
            var testCase = GetTestCase(testPassed);
            var subject = new TestCaseValidatorKat(testCase);

            var result = await subject.ValidateAsync(testCase);

            Assert.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ShouldNotValidateIfExpectedAndSuppliedDoNotMatch(bool testPassed)
        {
            var testCase = GetTestCase(testPassed);
            var subject = new TestCaseValidatorKat(GetTestCase(!testPassed));

            var result = await subject.ValidateAsync(testCase);

            Assert.That(result != null);
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
