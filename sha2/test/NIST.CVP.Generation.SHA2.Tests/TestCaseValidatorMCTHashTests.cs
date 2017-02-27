using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class TestCaseValidatorMCTHashTests
    {
        [Test]
        public void ShouldReturnPassWithAllMatches()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            var subject = new TestCaseValidatorMCTHash(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedDigest()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Digest =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Digest).ToBytes());

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual("failed", result.Result);
            Assert.IsTrue(result.Reason.ToLower().Contains("digest"), "Reason does not contain the expected digest");
            Assert.IsFalse(result.Reason.ToLower().Contains("message"), "Reason contains the unexpected value message");
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedMessage()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Message =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Message).ToBytes());

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual("failed", result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("digest"), "Reason contains the unexpected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("message"), "Reason contains the unexpected value message");
        }

        [Test]
        public void ShouldReturnReasonWithMultipleErrorReasons()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Digest =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Digest).ToBytes());
            supplied.ResultsArray[0].Message =
                new BitString(rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Message).ToBytes());

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = subject.Validate(supplied);

            Assert.IsTrue(result.Reason.ToLower().Contains("digest"), "Reason does not contain the expected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("message"), "Reason does not contain the expected value message");
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                ResultsArray = new List<AlgoArrayResponse>()
                {
                    new AlgoArrayResponse()
                    {
                        Message = new BitString("1234567890"),
                        Digest = new BitString("ABCDEF0ABCDEF0ABCDEF0ABCDEF0"),
                    }
                },
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
