using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_OFB.Tests
{
    [TestFixture]
    public class TestCaseValidatorMCTEncryptTests
    {
        [Test]
        public void ShouldReturnPassWithAllMatches()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedCipherText()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].CipherText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].CipherText).ToBytes());

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual("failed", result.Result);
            Assert.IsTrue(result.Reason.ToLower().Contains("cipher text"), "Reason does not contain the expected Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
            Assert.IsFalse(result.Reason.ToLower().Contains("iv"), "Reason contains the unexpected value IV");
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedPlainText()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].PlainText =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].PlainText).ToBytes());

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual("failed", result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("plain text"), "Reason does not contain the expected Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
            Assert.IsFalse(result.Reason.ToLower().Contains("iv"), "Reason contains the unexpected value IV");
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedKey()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Key =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Key).ToBytes());

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual("failed", result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("key"), "Reason does not contain the expected value Key");
            Assert.IsFalse(result.Reason.ToLower().Contains("iv"), "Reason contains the unexpected value IV");
        }


        [Test]
        public void ShouldReturnReasonOnMismatchedIV()
        {
            Random800_90 rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].IV =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].IV).ToBytes());

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual("failed", result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("cipher text"), "Reason contains the unexpected value Cipher Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("plain text"), "Reason contains the unexpected value Plain Text");
            Assert.IsFalse(result.Reason.ToLower().Contains("key"), "Reason contains the unexpected value Key");
            Assert.IsTrue(result.Reason.ToLower().Contains("iv"), "Reason does not contain the expected value IV");
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
            supplied.ResultsArray[0].Key =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Key).ToBytes());
            supplied.ResultsArray[0].IV =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].IV).ToBytes());

            TestCaseValidatorMCTEncrypt subject = new TestCaseValidatorMCTEncrypt(expected);

            var result = subject.Validate(supplied);

            Assert.IsTrue(result.Reason.ToLower().Contains("cipher text"), "Reason does not contain the expected value Cipher Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("plain text"), "Reason does not contain the expected value Plain Text");
            Assert.IsTrue(result.Reason.ToLower().Contains("key"), "Reason does not contain the expected value Key");
            Assert.IsTrue(result.Reason.ToLower().Contains("iv"), "Reason does not contain the expected value IV");
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
                        IV = new BitString("CAFECAFECAFECAFECAFECAFECAFECAFE"),
                        Key = new BitString("ABCDEF0ABCDEF0ABCDEF0ABCDEF0"),
                        PlainText = new BitString("FAF0FAF0FAF0FAF0FAF0")
                    }
                },
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
