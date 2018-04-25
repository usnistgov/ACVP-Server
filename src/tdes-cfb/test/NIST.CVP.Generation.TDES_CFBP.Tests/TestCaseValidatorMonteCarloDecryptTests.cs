using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES_CFBP;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.TDES_CFBP.Tests
{
    [TestFixture]
    public class TestCaseValidatorMonteCarloDecryptTests
    {
        [Test]
        public void ShouldReturnPassWithAllMatches()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedCipherText()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].CipherText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].CipherText).ToBytes());

            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.ToLower().Contains("cipher text"), "Reason does not contain the expected Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedPlainText()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].PlainText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].PlainText).ToBytes());

            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("plain text"), "Reason does not contain the expected Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedKey()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Keys =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Keys).ToBytes());

            TestCaseValidatorMonteCarloDecrypt subject = new TestCaseValidatorMonteCarloDecrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("key"), "Reason does not contain the expected value Key");
        }

        [Test]
        public void ShouldReturnReasonWithMultipleErrorReasons()
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

            var result = subject.Validate(supplied);

            Assert.IsTrue(result.Reason.ToLower().Contains("cipher text"), "Reason does not contain the expected value Cipher Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("plain text"), "Reason does not contain the expected value Plain Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("key"), "Reason does not contain the expected value Key");
        }

        [Test]
        public void ShouldFailDueToMissingResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray = null;

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = subject.Validate(suppliedResult);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailDueToMissingPlainTextInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.PlainText = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = subject.Validate(suppliedResult);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.PlainText)}"));
        }

        [Test]
        public void ShouldFailDueToMissingCipherTextInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.CipherText = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = subject.Validate(suppliedResult);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.CipherText)}"));
        }

        [Test]
        public void ShouldFailDueToMissingKeysInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Keys = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = subject.Validate(suppliedResult);

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Keys)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                ResultsArray = new List<AlgoArrayResponseWithIvs>()
                {
                    new AlgoArrayResponseWithIvs()
                    {
                        CipherText = new BitString("1234567890"),
                        Keys = new BitString("ABCDEF0ABCDEF0ABCDEF0ABCDEF0"),
                        PlainText = new BitString("FAF0FAF0FAF0FAF0FAF0")
                    }
                },
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
