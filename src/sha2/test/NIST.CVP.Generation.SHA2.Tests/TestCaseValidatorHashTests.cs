using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorHashTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorAFTHash(testCase);
            var result = subject.Validate(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldFailIfDigestDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorAFTHash(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Digest = new BitString("BEEFFACE");
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldShowDigestAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorAFTHash(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Digest = new BitString("BEEFFACE");
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Digest"));
        }

        [Test]
        public void ShouldFailIfDigestNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorAFTHash(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.Digest = null;

            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Digest)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase(int id = 1)
        {
            var testCase = new TestCase
            {
                Message = new BitString("ABCD"),
                Digest = new BitString("1234567890ABCDEF1234567890ABCDEF"),
                TestCaseId = id
            };

            return testCase;
        }
    }
}
