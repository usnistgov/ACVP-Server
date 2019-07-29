using System.Threading.Tasks;
using NIST.CVP.Generation.KMAC.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftTests
    {
        private TestCaseValidatorAft _subject;
        
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            _subject = new TestCaseValidatorAft(testCase, testGroup);
            var result = await _subject.ValidateAsync(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldFailIfMacDoesNotMatch()
        {
            var testMac = new BitString("D00000");

            var testCase = GetTestCase();
            var testGroup = GetTestGroup();

            _subject = new TestCaseValidatorAft(testCase, testGroup);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = testMac;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldShowMacAsReasonIfItDoesNotMatch()
        {
            var testMac = new BitString("D00000");

            var testCase = GetTestCase();
            var testGroup = GetTestGroup();

            _subject = new TestCaseValidatorAft(testCase, testGroup);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = testMac;
            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("MAC"));
        }

        [Test]
        public async Task ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            _subject = new TestCaseValidatorAft(testCase, testGroup);
            var suppliedResult = GetTestCase();

            suppliedResult.Mac = null;

            var result = await _subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Mac)} was not present in the {nameof(TestCase)}"));
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
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
