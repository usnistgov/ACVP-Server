﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFB
{
    [TestFixture]
    public class TestCaseValidatorEncryptTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var result = await subject.ValidateAsync(testCase);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        [Test]
        public async Task ShouldFailIfCipherTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = new BitString("D00000");
            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
        }

        [Test]
        public async Task ShouldShowCipherTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = new BitString("D00000");
            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Disposition.Failed == result.Result);
            Assert.That(result.Reason.Contains("Cipher Text"), Is.True);
        }

        [Test]
        [TestCase(100)]
        [TestCase(-2)]
        public void ShouldHaveTestCaseIdSetFromResult(int id)
        {
            var testCase = GetTestCase(id);
            var subject = new TestCaseValidatorEncrypt(testCase);
            Assert.That(subject.TestCaseId, Is.EqualTo(id));
        }

        [Test]
        public async Task ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.CipherText = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        private TestCase GetTestCase(int id = 1)
        {
            var testCase = new TestCase
            {
                CipherText = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = id
            };
            return testCase;
        }
    }
}
