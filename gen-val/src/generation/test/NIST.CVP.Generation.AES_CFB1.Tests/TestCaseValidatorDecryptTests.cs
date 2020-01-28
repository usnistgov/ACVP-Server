using System.Threading.Tasks;
using NIST.CVP.Generation.AES_CFB1.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorDecryptTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var result = await subject.ValidateAsync(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfPlainTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("11"));
            var result = await subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldShowPlainTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("11"));
            var result = await subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("Plain Text"));
        }

        [Test]
        public async Task ShouldFailIfPlainTextNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.PlainText = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase(bool failureTest = false)
        {
            var testCase = new TestCase
            {
                PlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1")),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
