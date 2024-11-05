using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CBC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CBC
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorMonteCarloEncryptTests
    {
        [Test]
        public async Task ShouldReturnPassWithAllMatches()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            TestCaseValidatorMonteCarloEncrypt subject = new TestCaseValidatorMonteCarloEncrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedCipherText()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].CipherText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].CipherText).ToBytes());

            TestCaseValidatorMonteCarloEncrypt subject = new TestCaseValidatorMonteCarloEncrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.ToLower().Contains("cipher text"), Is.True, "Reason does not contain the expected Cipher Text");
            Assert.That(result.Reason.ToLower().Contains("plain text"), Is.False, "Reason contains the unexpected value Plain Text");
            Assert.That(result.Reason.ToLower().Contains("key"), Is.False, "Reason contains the unexpected value Key");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedPlainText()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].PlainText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].PlainText).ToBytes());

            TestCaseValidatorMonteCarloEncrypt subject = new TestCaseValidatorMonteCarloEncrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.ToLower().Contains("cipher text"), Is.False, "Reason contains the unexpected value Cipher Text");
            Assert.That(result.Reason.ToLower().Contains("plain text"), Is.True, "Reason does not contain the expected Plain Text");
            Assert.That(result.Reason.ToLower().Contains("key"), Is.False, "Reason contains the unexpected value Key");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedKey()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Keys =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Keys).ToBytes());

            TestCaseValidatorMonteCarloEncrypt subject = new TestCaseValidatorMonteCarloEncrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.ToLower().Contains("cipher text"), Is.False, "Reason contains the unexpected value Cipher Text");
            Assert.That(result.Reason.ToLower().Contains("plain text"), Is.False, "Reason contains the unexpected value Plain Text");
            Assert.That(result.Reason.ToLower().Contains("key"), Is.True, "Reason does not contain the expected value Key");
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

            TestCaseValidatorMonteCarloEncrypt subject = new TestCaseValidatorMonteCarloEncrypt(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.That(result.Reason.ToLower().Contains("cipher text"), Is.True, "Reason does not contain the expected value Cipher Text");
            Assert.That(result.Reason.ToLower().Contains("plain text"), Is.True, "Reason does not contain the expected value Plain Text");
            Assert.That(result.Reason.ToLower().Contains("key"), Is.True, "Reason does not contain the expected value Key");
        }

        [Test]
        public async Task ShouldFailDueToMissingResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray = null;

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        [Test]
        public async Task ShouldFailDueToMissingPlainTextInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.PlainText = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.PlainText)}"), Is.True);
        }

        [Test]
        public async Task ShouldFailDueToMissingCipherTextInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.CipherText = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.CipherText)}"), Is.True);
        }

        [Test]
        public async Task ShouldFailDueToMissingKeysInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Keys = null);

            var subject = new TestCaseValidatorMonteCarloDecrypt(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Keys)}"), Is.True);
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
                        IV = new BitString("CAFEFACE")
                    }
                },
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
