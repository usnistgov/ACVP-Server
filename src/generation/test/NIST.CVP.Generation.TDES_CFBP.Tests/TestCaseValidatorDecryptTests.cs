using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.TDES_CFBP.v1_0;
using NIST.CVP.Math;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture]
    public class TestCaseValidatorDecryptTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var result = await subject.ValidateAsync(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfPlainTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString("D00000");
            var result = await subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldShowPlainTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString("D00000");
            var result = await subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assume.That(Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("Plain Text"));
        }

        [Test]
        [TestCase(100)]
        [TestCase(-2)]
        public void ShouldHaveTestCaseIdSetFromResult(int id)
        {
            var testCase = GetTestCase(id);
            var subject = new TestCaseValidatorDecrypt(testCase);
            Assert.AreEqual(id, subject.TestCaseId);
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
            Assume.That(Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase(int id = 1)
        {
            var testCase = new TestCase
            {
                PlainText = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = id
            };
            return testCase;
        }
    }
}
