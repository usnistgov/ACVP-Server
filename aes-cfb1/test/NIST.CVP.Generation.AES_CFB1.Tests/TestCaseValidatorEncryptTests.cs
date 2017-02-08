using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.Tests
{
    [TestFixture]
    public class TestCaseValidatorEncryptTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var result = subject.Validate(testCase);
            Assume.That(result != null);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldFailIfCipherTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("11");
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual("failed", result.Result);
        }

        [Test]
        public void ShouldShowCipherTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("11");
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);
            Assert.IsTrue(result.Reason.Contains("Cipher Text"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("1"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
