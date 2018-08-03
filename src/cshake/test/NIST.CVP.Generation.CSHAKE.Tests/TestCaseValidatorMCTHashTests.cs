using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CSHAKE.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorMCTHashTests
    {
        [Test]
        public async Task ShouldReturnPassWithAllMatches()
        {
            var expected = GetTestCase();
            var supplied = GetTestCase();
            var subject = new TestCaseValidatorMCTHash(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedDigest()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Digest = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Digest);

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.ToLower().Contains("digest"), "Reason does not contain the expected digest");
            Assert.IsFalse(result.Reason.ToLower().Contains("message"), "Reason contains the unexpected value message");
            Assert.IsFalse(result.Reason.ToLower().Contains("customization"), "Reason contains the unexpected value customization");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedMessage()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Message = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Message);

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("digest"), "Reason contains the unexpected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("message"), "Reason contains the unexpected value message");
            Assert.IsFalse(result.Reason.ToLower().Contains("customization"), "Reason contains the unexpected value customization");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedCustomization()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Customization = rand.GetRandomString(supplied.ResultsArray[0].Customization.Length + 1);

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("digest"), "Reason contains the unexpected value digest");
            Assert.IsFalse(result.Reason.ToLower().Contains("message"), "Reason contains the unexpected value message");
            Assert.IsTrue(result.Reason.ToLower().Contains("customization"), "Reason does not contain the expected value customization");
        }

        [Test]
        public async Task ShouldReturnReasonWithMultipleErrorReasons()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Digest = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Digest);
            supplied.ResultsArray[0].Message = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Message);
            supplied.ResultsArray[0].Customization = rand.GetRandomString(supplied.ResultsArray[0].Customization.Length + 1);

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.IsTrue(result.Reason.ToLower().Contains("digest"), "Reason does not contain the expected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("message"), "Reason does not contain the expected value message");
            Assert.IsTrue(result.Reason.ToLower().Contains("customization"), "Reason does not contain the expected value customization");
        }

        [Test]
        public async Task ShouldFailDueToMissingResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray = null;

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingMessageInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Message = null);

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithCustomization.Message)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingDigestInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Digest = null);

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithCustomization.Digest)}"));
        }

        [Test]
        public async Task ShouldFailDueToMissingCustomizationInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Customization = null);

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithCustomization.Customization)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                ResultsArray = new List<AlgoArrayResponseWithCustomization>()
                {
                    new AlgoArrayResponseWithCustomization()
                    {
                        Message = new BitString("1234567890"),
                        Digest = new BitString("ABCDEF0ABCDEF0ABCDEF0ABCDEF0"),
                        Customization = "custom"
                    }
                },
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
