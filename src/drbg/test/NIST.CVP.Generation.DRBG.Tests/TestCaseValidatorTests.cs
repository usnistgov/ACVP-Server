using NIST.CVP.Math;
using NIST.CVP.Generation.DRBG;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var result = subject.Validate(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldFailIfCipherTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.ReturnedBits = new BitString("D00000");
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldShowCipherTextAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.ReturnedBits = new BitString("D00000");
            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("Returned Bits"));
        }

        [Test]
        public void ShouldFailIfReturnedBitsNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.ReturnedBits = null;

            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.ReturnedBits)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                ReturnedBits = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
