using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CFB1;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.Tests
{
    [TestFixture, UnitTest]
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
        
        [Test]
        public void ShouldFailDueToMissingResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray = null;

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = subject.Validate(suppliedResult);

            Assert.AreEqual("failed", result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorEncrypt(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.CipherText = null;

            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}"));
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
