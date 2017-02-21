using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CBC.Tests
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

        [Test]
        [TestCase(100)]
        [TestCase(-2)]
        public void ShouldHaveTestCaseIdSetFromResult(int id)
        {
            var testCase = GetTestCase(id);
            var subject = new TestCaseValidatorDecrypt(testCase);
            Assert.AreEqual(id, subject.TestCaseId);
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
