using System.Threading.Tasks;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorMvtTests
    {
        private TestCaseValidatorMvt _subject;

        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            _subject = new TestCaseValidatorMvt(testCase, testGroup);
            var result = await _subject.ValidateAsync(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfMacVerifiedDoesNotMatch()
        {
            var testMacVerified = false;

            var testCase = GetTestCase();
            var testGroup = GetTestGroup();

            _subject = new TestCaseValidatorMvt(testCase, testGroup);
            var suppliedResult = GetTestCase();
            suppliedResult.MacVerified = testMacVerified;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldShowMacVerifiedAsReasonIfItDoesNotMatch()
        {
            var testMacVerified = false;

            var testCase = GetTestCase();
            var testGroup = GetTestGroup();

            _subject = new TestCaseValidatorMvt(testCase, testGroup);
            var suppliedResult = GetTestCase();
            suppliedResult.MacVerified = testMacVerified;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("MAC Verification"));
        }

        [Test]
        public async Task ShouldFailIfMacVerifiedNotPresent()
        {
            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            _subject = new TestCaseValidatorMvt(testCase, testGroup);
            var suppliedResult = GetTestCase();

            suppliedResult.MacVerified = null;

            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.MacVerified)} was not present in the {nameof(TestCase)}"));
        }

        private TestGroup GetTestGroup()
        {
            var testGroup = new TestGroup
            {
                KeyLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 256, 512)),
                MessageLength = 128
            };

            return testGroup;
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                Message = new BitString("AADAADAADAAD"),
                Mac = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                MacVerified = true,
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
