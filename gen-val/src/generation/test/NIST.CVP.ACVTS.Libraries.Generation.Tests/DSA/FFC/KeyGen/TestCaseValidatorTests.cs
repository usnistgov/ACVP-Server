using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.KeyGen
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodKey()
        {
            var deferredMock = GetDeferredMock(true);

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), deferredMock.Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            deferredMock.Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var deferredMock = GetDeferredMock(false);

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), deferredMock.Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            deferredMock.Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1
            };
        }

        private TestCase GetResultTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                Key = new FfcKeyPair(1, 2)
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224,
            };
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, FfcKeyPairValidateResult>> GetDeferredMock(bool shouldPass)
        {
            var goodResult = Task.FromResult(new FfcKeyPairValidateResult());
            var badResult = Task.FromResult(new FfcKeyPairValidateResult("fail"));

            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, FfcKeyPairValidateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
        }
    }
}
