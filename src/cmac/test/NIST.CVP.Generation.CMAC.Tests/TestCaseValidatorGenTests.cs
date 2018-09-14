using System.Threading.Tasks;
using NIST.CVP.Generation.CMAC;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.Tests
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
            Assume.That((bool) (result != null));
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfMacDoesNotMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = new BitString("D00000");
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That((bool) (result != null));
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldShowMacAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = new BitString("D00000");
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That((bool) (result != null));
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue((bool) result.Reason.Contains("MAC"));
        }

        [Test]
        public async Task ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.Mac = null;

            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That((bool) (result != null));
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue((bool) result.Reason.Contains($"{nameof(suppliedResult.Mac)} was not present in the {typeof(TestCase)}"));
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
