using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF.v1_0
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

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed), result.Reason);
        }

        [Test]
        public async Task ShouldFailIfKeyOutDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();
            suppliedResult.KeyOut = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldShowKeyOutAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();
            suppliedResult.KeyOut = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.That(result.Reason.Contains("KeyOut"), Is.True);
        }

        [Test]
        public async Task ShouldFailIfKeyOutNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase, GetTestGroup(), GetResolver().Object);
            var suppliedResult = GetTestCase();

            suppliedResult.KeyOut = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.KeyOut)} was not present in the {nameof(TestCase)}"), Is.True);
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
                .Returns(Task.FromResult(new KdfResult { KeyOut = new BitString("ABCDEF0123456789ABCDEF0123456789") }));
            return mock;
        }
    }
}
