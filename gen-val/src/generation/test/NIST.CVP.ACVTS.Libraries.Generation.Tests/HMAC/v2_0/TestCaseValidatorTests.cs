using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v2_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.HMAC.v2_0
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        private TestCaseValidator _subject;

        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidator(testCase);
            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result, Is.Not.EqualTo(null));
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldFailIfMacDoesNotMatch()
        {
            var testMac = new BitString("D00000");
            var testCase = GetTestCase();

            _subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = testMac;
            var result = await _subject.ValidateAsync(suppliedResult);
            
            Assert.That(result, Is.Not.EqualTo(null));
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldShowMacAsReasonIfItDoesNotMatch()
        {
            var testMac = new BitString("D00000");
            var testCase = GetTestCase();

            _subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = testMac;
            var result = await _subject.ValidateAsync(suppliedResult);
            
            Assert.That(result, Is.Not.EqualTo(null));
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason, Does.Contain("MAC"));
        }

        [Test]
        public async Task ShouldFailIfTagNotPresent()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.Mac = null;

            var result = await _subject.ValidateAsync(suppliedResult);
            Assert.That(result, Is.Not.EqualTo(null));
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));

            Assert.That(result.Reason, Does.Contain($"{nameof(suppliedResult.Mac)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                Message = new BitString("AADAADAADAAD"),
                Mac = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
