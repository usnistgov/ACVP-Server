using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KMAC
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
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldFailIfMacVerifiedDoesNotMatch()
        {
            var testMacVerified = false;

            var testCase = GetTestCase();
            var testGroup = GetTestGroup();

            _subject = new TestCaseValidatorMvt(testCase, testGroup);
            var suppliedResult = GetTestCase();
            suppliedResult.TestPassed = testMacVerified;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldShowMacVerifiedAsReasonIfItDoesNotMatch()
        {
            var testMacVerified = false;

            var testCase = GetTestCase();
            var testGroup = GetTestGroup();

            _subject = new TestCaseValidatorMvt(testCase, testGroup);
            var suppliedResult = GetTestCase();
            suppliedResult.TestPassed = testMacVerified;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains("MAC Verification"), Is.True);
        }

        [Test]
        public async Task ShouldFailIfMacVerifiedNotPresent()
        {
            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            _subject = new TestCaseValidatorMvt(testCase, testGroup);
            var suppliedResult = GetTestCase();

            suppliedResult.TestPassed = null;

            var result = await _subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.TestPassed)} was not present in the {nameof(TestCase)}"), Is.True);
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
                TestPassed = true,
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
