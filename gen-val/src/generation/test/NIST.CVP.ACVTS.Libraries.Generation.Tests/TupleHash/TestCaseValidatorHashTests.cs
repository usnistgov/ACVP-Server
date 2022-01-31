using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.TupleHash.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TupleHash
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorHashTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var result = await subject.ValidateAsync(testCase);
            Assert.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfDigestDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Digest = new BitString("BEEFFACE");
            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldShowDigestAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Digest = new BitString("BEEFFACE");
            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Digest"));
        }

        [Test]
        public async Task ShouldFailIfDigestNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.Digest = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Digest)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase(int id = 1)
        {
            var testCase = new TestCase
            {
                Tuple = new List<BitString>(new BitString[] { new BitString("ABCD") }),
                Digest = new BitString("1234567890ABCDEF1234567890ABCDEF"),
                TestCaseId = id
            };

            return testCase;
        }
    }
}
