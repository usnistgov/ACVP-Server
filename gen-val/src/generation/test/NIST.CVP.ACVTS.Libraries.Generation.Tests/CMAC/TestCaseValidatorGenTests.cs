using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGenTests
    {
        private TestCaseValidatorGen _subject;

        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen(testCase);
            var result = await _subject.ValidateAsync(testCase);
            Assert.That((bool)(result != null));
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldFailIfMacDoesNotMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = new BitString("D00000");
            var result = await _subject.ValidateAsync(suppliedResult);
            Assert.That((bool)(result != null));
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldShowMacAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = new BitString("D00000");
            var result = await _subject.ValidateAsync(suppliedResult);
            Assert.That((bool)(result != null));
            Assert.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.That((bool)result.Reason.Contains("MAC"), Is.True);
        }

        [Test]
        public async Task ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.Mac = null;

            var result = await _subject.ValidateAsync(suppliedResult);
            Assert.That((bool)(result != null));
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.That((bool)result.Reason.Contains($"{nameof(suppliedResult.Mac)} was not present in the {typeof(TestCase)}"), Is.True);
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
