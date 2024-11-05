using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash;
using NIST.CVP.ACVTS.Libraries.Generation.TupleHash.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TupleHash
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

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
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

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.ToLower().Contains("digest"), Is.True, "Reason does not contain the expected digest");
            Assert.That(result.Reason.ToLower().Contains("tuple"), Is.False, "Reason contains the unexpected value tuple");
            Assert.That(result.Reason.ToLower().Contains("customization"), Is.False, "Reason does not contain the expected value customization");
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

            Assert.That(result.Reason.ToLower().Contains("digest"), Is.True, "Reason does not contain the expected value digest");
            Assert.That(result.Reason.ToLower().Contains("tuple"), Is.False, "Reason does not contain the expected value tuple");
            Assert.That(result.Reason.ToLower().Contains("customization"), Is.False, "Reason does not contain the expected value customization");
        }

        [Test]
        public async Task ShouldFailDueToMissingResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray = null;

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        [Test]
        public async Task ShouldFailDueToMissingDigestInResultsArray()
        {
            var expected = GetTestCase();
            var suppliedResult = GetTestCase();

            suppliedResult.ResultsArray.ForEach(fe => fe.Digest = null);

            var subject = new TestCaseValidatorMCTHash(expected);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Digest)}"), Is.True);
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
