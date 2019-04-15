using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Generation.TupleHash.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TupleHash.Tests
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
            Assert.IsFalse(result.Reason.ToLower().Contains("tuple"), "Reason contains the unexpected value tuple");
            Assert.IsFalse(result.Reason.ToLower().Contains("customization"), "Reason does not contain the expected value customization");
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
            Assert.IsFalse(result.Reason.ToLower().Contains("digest"), "Reason does not contain the expected digest");
            Assert.IsFalse(result.Reason.ToLower().Contains("tuple"), "Reason contains the unexpected value tuple");
            Assert.IsTrue(result.Reason.ToLower().Contains("customization"), "Reason does not contain the expected value customization");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedSimpleTuple()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Tuple = new List<BitString>(new BitString[] { rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Tuple.ElementAt(0)) });

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("digest"), "Reason contains the unexpected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("tuple"), "Reason contains the unexpected value tuple");
            Assert.IsFalse(result.Reason.ToLower().Contains("customization"), "Reason does not contain the expected value customization");
        }

        [Test]
        public async Task ShouldReturnReasonOnMismatchedComplexTuple()
        {
            var rand = new Random800_90();
            var expected = GetTestCaseComplexTuple();
            var supplied = GetTestCaseComplexTuple();
            supplied.ResultsArray[0].Tuple.Add(new BitString(""));

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsFalse(result.Reason.ToLower().Contains("digest"), "Reason contains the unexpected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("tuple"), "Reason contains the unexpected value tuple");
            Assert.IsFalse(result.Reason.ToLower().Contains("customization"), "Reason does not contain the expected value customization");
        }

        [Test]
        public async Task ShouldReturnReasonWithMultipleErrorReasons()
        {
            var rand = new Random800_90();
            var expected = GetTestCase();
            var supplied = GetTestCase();
            supplied.ResultsArray[0].Digest = rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Digest);
            supplied.ResultsArray[0].Tuple = new List<BitString>(new BitString[] { rand.GetDifferentBitStringOfSameSize(supplied.ResultsArray[0].Tuple.ElementAt(0)) });
            supplied.ResultsArray[0].Customization = rand.GetRandomString(supplied.ResultsArray[0].Customization.Length + 1);

            var subject = new TestCaseValidatorMCTHash(expected);

            var result = await subject.ValidateAsync(supplied);

            Assert.IsTrue(result.Reason.ToLower().Contains("digest"), "Reason does not contain the expected value digest");
            Assert.IsTrue(result.Reason.ToLower().Contains("tuple"), "Reason does not contain the expected value tuple");
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
        public async Task ShouldFailDueToMissingTupleInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Tuple = null);

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Tuple)}"));
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
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Digest)}"));
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
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Customization)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                ResultsArray = new List<AlgoArrayResponse>()
                {
                    new AlgoArrayResponse()
                    {
                        Tuple = new List<BitString>(new BitString[]{ new BitString("1234567890") }),
                        Digest = new BitString("ABCDEF0ABCDEF0ABCDEF0ABCDEF0"),
                        Customization = "custom"
                    }
                },
                TestCaseId = 1
            };
            return testCase;
        }

        private TestCase GetTestCaseComplexTuple()
        {
            var testCase = new TestCase
            {
                ResultsArray = new List<AlgoArrayResponse>()
                {
                    new AlgoArrayResponse()
                    {
                        Tuple = new List<BitString>(new BitString[]{ new BitString("1234567890"), new BitString("1234"), new BitString(0) }),
                        Digest = new BitString("ABCDEF0ABCDEF0ABCDEF0ABCDEF0"),
                        Customization = "custom"
                    }
                },
                TestCaseId = 2
            };
            return testCase;
        }
    }
}
