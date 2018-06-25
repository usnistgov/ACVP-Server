using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorMCTHashTests
    {
        [Test]
        public void ShouldReturnPassWithAllMatches()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            var subject = new TestCaseValidatorMCTHash(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedDigest()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Digest = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Digest);

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.ToLower().Contains("digest"), "Reason does not contain the expected digest");
            Assert.IsFalse(result.Reason.ToLower().Contains("message"), "Reason contains the unexpected value message");
        }

        [Test]
        public void ShouldReturnReasonOnMismatchedMessage()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Message = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Message);

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = subject.Validate(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("digest"), "Reason contains the unexpected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("message"), "Reason contains the unexpected value message");
        }

        [Test]
        public void ShouldReturnReasonWithMultipleErrorReasons()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Digest = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Digest);
            supplied.ResultsArray[0].Message = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Message);

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = subject.Validate(supplied);

            Assert.IsTrue(result.Reason.ToLower().Contains("digest"), "Reason does not contain the expected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("message"), "Reason does not contain the expected value message");
        }

        [Test]
        public void ShouldFailDueToMissingResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray = null;

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = subject.Validate(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailDueToMissingMessageInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Message = null);

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = subject.Validate(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Message)}"));
        }

        [Test]
        public void ShouldFailDueToMissingDigestInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Digest = null);

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = subject.Validate(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Digest)}"));
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
