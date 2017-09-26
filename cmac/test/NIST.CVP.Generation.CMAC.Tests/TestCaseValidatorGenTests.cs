using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NIST.CVP.Generation.CMAC;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGenTests
    {
        private TestCaseValidatorGen<TestCase> _subject;
        
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen<TestCase>(testCase);
            var result = _subject.Validate(testCase);
            Assume.That(result != null);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldFailIfMacDoesNotMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen<TestCase>(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual("failed", result.Result);
        }

        [Test]
        public void ShouldShowMacAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen<TestCase>(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);
            Assert.IsTrue(result.Reason.Contains("MAC"));
        }

        [Test]
        public void ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            _subject = new TestCaseValidatorGen<TestCase>(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.Mac = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Mac)} was not present in the {typeof(TestCase)}"));
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
