using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.FF
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorEncryptTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var result = await subject.ValidateAsync(testCase, false);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        [Test]
        public async Task ShouldFailIfCipherTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = "aa";
            var result = await subject.ValidateAsync(suppliedResult, false);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
        }

        [Test]
        public async Task ShouldShowCipherTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = "aa";
            var result = await subject.ValidateAsync(suppliedResult, false);
            Assert.That(result != null);
            Assert.That(Disposition.Failed == result.Result);
            Assert.That(result.Reason.Contains("Cipher Text"), Is.True);
        }

        [Test]
        public async Task ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.CipherText = null;

            var result = await subject.ValidateAsync(suppliedResult, false);
            Assert.That(result != null);
            Assert.That(Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                CipherText = "abcde",
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
