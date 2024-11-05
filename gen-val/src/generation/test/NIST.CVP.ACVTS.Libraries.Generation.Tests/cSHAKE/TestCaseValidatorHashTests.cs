using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.cSHAKE
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorHashTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var result = await subject.ValidateAsync(testCase);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldFailIfDigestDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Digest = new BitString("BEEFFACE");
            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldShowDigestAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Digest = new BitString("BEEFFACE");
            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains("Digest"), Is.True);
        }

        [Test]
        public async Task ShouldFailIfDigestNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.Digest = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.Digest)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        private TestCase GetTestCase(int id = 1)
        {
            var testCase = new TestCase
            {
                Message = new BitString("ABCD"),
                Digest = new BitString("1234567890ABCDEF1234567890ABCDEF"),
                TestCaseId = id
            };

            return testCase;
        }
    }
}
