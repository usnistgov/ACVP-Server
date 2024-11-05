using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX963;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ANSIX943
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);

            var result = await subject.ValidateAsync(testCase);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed), result.Reason);
        }

        [Test]
        public async Task ShouldFailIfValueDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.KeyData = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains(nameof(suppliedResult.KeyData)), Is.True);
        }

        [Test]
        public async Task ShouldShowValueAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.KeyData = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.That(result.Reason.Contains(nameof(suppliedResult.KeyData)), Is.True);
        }

        [Test]
        public async Task ShouldFailIfValueNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.KeyData = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.KeyData)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                KeyData = new BitString("01ABCDEF0123456789ABCDEF012345678901"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
