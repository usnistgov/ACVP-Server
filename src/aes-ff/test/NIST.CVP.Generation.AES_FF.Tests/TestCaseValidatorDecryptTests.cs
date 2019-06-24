using NIST.CVP.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_FF.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorDecryptTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var result = await subject.ValidateAsync(testCase, false);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfPlainTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = "abc";
            var result = await subject.ValidateAsync(suppliedResult, false);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldShowPlainTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = "abc";
            var result = await subject.ValidateAsync(suppliedResult, false);
            Assume.That(result != null);
            Assume.That(Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("Plain Text"));
        }

        [Test]
        public async Task ShouldFailIfPlainTextNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.PlainText = null;

            var result = await subject.ValidateAsync(suppliedResult, false);
            Assume.That(result != null);
            Assume.That(Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                PlainText = "abcd",
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
