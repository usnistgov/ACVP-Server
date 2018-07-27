using System.Threading.Tasks;
using Moq;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KDF.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorEncryptTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);

            var result = await subject.ValidateAsync(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result, result.Reason);
        }

        [Test]
        public async Task ShouldFailIfKeyOutDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();
            suppliedResult.KeyOut = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldShowKeyOutAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();
            suppliedResult.KeyOut = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("KeyOut"));
        }

        [Test]
        public async Task ShouldFailIfKeyOutNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();

            suppliedResult.KeyOut = null;

            var result = await subject.ValidateAsync(suppliedResult);
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

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, KdfResult>> GetResolver()
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, KdfResult>>();
            mock.Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new KdfResult{ KeyOut = new BitString("ABCDEF0123456789ABCDEF0123456789") }));
            return mock;
        }
    }
}
