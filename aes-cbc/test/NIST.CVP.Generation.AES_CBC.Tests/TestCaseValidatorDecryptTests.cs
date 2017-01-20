using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CBC.Tests
{
    [TestFixture]
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
