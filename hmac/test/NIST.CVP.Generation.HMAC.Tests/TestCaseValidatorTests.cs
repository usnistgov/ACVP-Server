using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        private TestCaseValidator _subject;
        
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            _subject = new TestCaseValidator(testCase, testGroup);
            var result = _subject.Validate(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldFailIfMacDoesNotMatch()
        {
            var testMac = new BitString("D00000");

            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            testGroup.MacLength = testMac.BitLength;

            _subject = new TestCaseValidator(testCase, testGroup);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = testMac;
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldShowMacAsReasonIfItDoesNotMatch()
        {
            var testMac = new BitString("D00000");

            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            testGroup.MacLength = testMac.BitLength;

            _subject = new TestCaseValidator(testCase, testGroup);
            var suppliedResult = GetTestCase();
            suppliedResult.Mac = testMac;
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("MAC"));
        }

        [Test]
        public void ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            var testGroup = GetTestGroup();
            _subject = new TestCaseValidator(testCase, testGroup);
            var suppliedResult = GetTestCase();

            suppliedResult.Mac = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Mac)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldPassWithMacsDifferingAfterBitLength()
        {
            var testCaseExpected = GetTestCase();
            var testCaseSupplied = GetTestCase();
            var testGroup = GetTestGroup();
            testGroup.MacLength = 9;

            testCaseExpected.Mac = new BitString("F500CAFECAFE");
            testCaseSupplied.Mac = new BitString("F500FACEFACE");

            _subject = new TestCaseValidator(testCaseExpected, testGroup);
            var result = _subject.Validate(testCaseSupplied);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        private TestGroup GetTestGroup()
        {
            var testGroup = new TestGroup
            {
                ShaMode = ModeValues.SHA1,
                ShaDigestSize = DigestSizes.d160,
                MacLength = 80,
                KeyLength = 128,
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
