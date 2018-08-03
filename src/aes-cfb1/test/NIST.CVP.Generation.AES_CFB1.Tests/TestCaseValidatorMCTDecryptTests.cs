using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB1.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorMCTDecryptTests
    {
        [Test]
        public async Task ShouldReturnPassWithAllMatches()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            TestCaseValidatorMCTDecrypt subject = new TestCaseValidatorMCTDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedCipherText()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].CipherText = supplied.ResultsArray[0].CipherText.NOT();

            TestCaseValidatorMCTDecrypt subject = new TestCaseValidatorMCTDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.ToLower().Contains("cipher text"), "Reason does not contain the expected Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
            Assert.IsFalse(result.Reason.ToLower().Contains("iv"), "Reason contains the unexpected value IV");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedPlainText()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].PlainText = supplied.ResultsArray[0].PlainText.NOT();
                
            TestCaseValidatorMCTDecrypt subject = new TestCaseValidatorMCTDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("plain text"), "Reason does not contain the expected Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
            Assert.IsFalse(result.Reason.ToLower().Contains("iv"), "Reason contains the unexpected value IV");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedKey()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Key =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Key).ToBytes());

            TestCaseValidatorMCTDecrypt subject = new TestCaseValidatorMCTDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("key"), "Reason does not contain the expected value Key");
            Assert.IsFalse(result.Reason.ToLower().Contains("iv"), "Reason contains the unexpected value IV");
        }


        [Test]
        public async Task ShouldReturnReasonOnMismatchedIV()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].IV =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].IV).ToBytes());

            TestCaseValidatorMCTDecrypt subject = new TestCaseValidatorMCTDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
            Assert.IsTrue(result.Reason.ToLower().Contains("iv"), "Reason does not contain the expected value IV");
        }

        [Test]
        public async Task ShouldReturnReasonWithMultipleErrorReasons()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].CipherText = supplied.ResultsArray[0].CipherText.NOT();
            supplied.ResultsArray[0].PlainText = supplied.ResultsArray[0].PlainText.NOT();
            supplied.ResultsArray[0].Key =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Key).ToBytes());
            supplied.ResultsArray[0].IV =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].IV).ToBytes());

            TestCaseValidatorMCTDecrypt subject = new TestCaseValidatorMCTDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);
            
            Assert.IsTrue(result.Reason.ToLower().Contains("cipher text"), "Reason does not contain the expected value Cipher Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("plain text"), "Reason does not contain the expected value Plain Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("key"), "Reason does not contain the expected value Key");
            Assert.IsTrue(result.Reason.ToLower().Contains("iv"), "Reason does not contain the expected value IV");
        }


        [Test]
        public async Task ShouldFailDueToMissingResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray = null;

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingKeyInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Key = null);

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason
                .Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Key)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingIvInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.IV = null);

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason
                .Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.IV)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingPlainTextInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.PlainText = null);

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason
                .Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.PlainText)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingCipherTextInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.CipherText = null);

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason
                .Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.CipherText)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                ResultsArray = new List<AlgoArrayResponse>()
                {
                    new AlgoArrayResponse()
                    {
                        CipherText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("1")),
                        IV = new BitString("CAFECAFECAFECAFECAFECAFECAFECAFE"),
                        Key = new BitString("ABCDEF0ABCDEF0ABCDEF0ABCDEF0"),
                        PlainText = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0sReversed("0"))
                    }
                },
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
