using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFBP
{
    [TestFixture]
    public class TestCaseValidatorMonteCarloDecryptTests
    {
        [Test]
        public async Task ShouldReturnPassWithAllMatches()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedCipherText()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].CipherText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].CipherText).ToBytes());

            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.ToLower().Contains("cipher text"), "Reason does not contain the expected Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedPlainText()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].PlainText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].PlainText).ToBytes());

            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("plain text"), "Reason does not contain the expected Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedKey()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Keys =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Keys).ToBytes());

            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("key"), "Reason does not contain the expected value Key");
        }

        [Test]
        public async Task ShouldReturnReasonWithMultipleErrorReasons()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].CipherText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].CipherText).ToBytes());
            supplied.ResultsArray[0].PlainText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].PlainText).ToBytes());
            supplied.ResultsArray[0].Keys =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Keys).ToBytes());

            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.IsTrue(result.Reason.ToLower().Contains("cipher text"), "Reason does not contain the expected value Cipher Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("plain text"), "Reason does not contain the expected value Plain Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("key"), "Reason does not contain the expected value Key");
        }

        [Test]
        public async Task ShouldFailDueToMissingResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray = null;

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingPlainTextInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.PlainText = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.PlainText)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingCipherTextInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.CipherText = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.CipherText)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingKeysInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Keys = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Keys)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                ResultsArray = new List<AlgoArrayResponse>()
                {
                    new AlgoArrayResponse()
                    {
                        CipherText = new BitString("1234567890"),
                        Keys = new BitString("ABCDEF0ABCDEF0ABCDEF0ABCDEF0"),
                        PlainText = new BitString("FAF0FAF0FAF0FAF0FAF0"),
                        IV = new BitString("00000000000000000000000000000000"),
                    }
                },
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
