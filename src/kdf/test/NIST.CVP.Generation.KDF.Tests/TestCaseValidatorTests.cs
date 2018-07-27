using Moq;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KDF.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorEncryptTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result, result.Reason);
        }

        [Test]
        public void ShouldFailIfKeyOutDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();
            suppliedResult.KeyOut = new BitString("D00000");

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldShowKeyOutAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();
            suppliedResult.KeyOut = new BitString("D00000");

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("KeyOut"));
        }

        [Test]
        public void ShouldFailIfKeyOutNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();

            suppliedResult.KeyOut = null;

            var result = subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.KeyOut)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                KeyOut = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                FixedData = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = 1
            };
            return testCase;
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                KdfMode = KdfModes.Counter,
                MacMode = MacModes.HMAC_SHA384,
                KeyOutLength = 128
            };
        }

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KdfResult>> GetResolver()
        {
            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KdfResult>>();
            mock.Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new KdfResult{ KeyOut = new BitString("ABCDEF0123456789ABCDEF0123456789") });
            return mock;
        }
    }
}
