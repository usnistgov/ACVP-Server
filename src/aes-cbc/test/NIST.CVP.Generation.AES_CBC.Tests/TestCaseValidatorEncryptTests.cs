﻿using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CBC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorEncryptTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var result = subject.Validate(testCase, false);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldFailIfCipherTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = new BitString("D00000");
            var result = subject.Validate(suppliedResult, false);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldShowCipherTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = new BitString("D00000");
            var result = subject.Validate(suppliedResult, false);
            Assume.That(result != null);
            Assume.That(Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("Cipher Text"));
        }

        [Test]
        public void ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.CipherText = null;

            var result = subject.Validate(suppliedResult, false);
            Assume.That(result != null);
            Assume.That(Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                CipherText = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}