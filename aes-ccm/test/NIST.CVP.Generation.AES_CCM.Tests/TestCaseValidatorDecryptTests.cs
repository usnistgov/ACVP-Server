using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorDecryptTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var result = subject.Validate(testCase);
            Assume.That(result != null);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldFailIfPlainTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString("D00000");
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual("failed", result.Result);
        }

        [Test]
        public void ShouldShowPlainTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString("D00000");
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);
            Assert.IsTrue(result.Reason.Contains("Plain Text"));
        }
        
        [Test]
        public void ShouldFailIfFailedTestDoesNotMatch()
        {
            var testCase = GetTestCase(true);
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase(false);
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);
            Assert.IsTrue(result.Reason.Contains("tag"));
        }

        [Test]
        public void ShouldNotFailTestDueToBadPlainTextWhenTestIsExpectedToBeFailureTest()
        {
            var testCase = GetTestCase(true);
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase(true);
            suppliedResult.PlainText = new BitString(0);
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldFailIfPlainTextNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorDecrypt(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.PlainText = null;

            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase(bool failureTest = false)
        {
            var testCase = new TestCase
            {
                FailureTest = failureTest,
                PlainText = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
