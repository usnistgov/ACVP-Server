using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS
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
            suppliedResult.KeyBlock = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains(nameof(suppliedResult.KeyBlock)), Is.True);
        }

        [Test]
        public async Task ShouldShowValueAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.MasterSecret = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.That(result.Reason.Contains(nameof(suppliedResult.MasterSecret)), Is.True);
        }

        [Test]
        public async Task ShouldFailIfValueNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.KeyBlock = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.KeyBlock)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                KeyBlock = new BitString("01ABCDEF0123456789ABCDEF012345678901"),
                MasterSecret = new BitString("02ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
