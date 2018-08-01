using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodSignature()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredMock(true).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredMock(false).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                Message = new BitString("BEEFFACE")
            };
        }

        private TestCase GetResultTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                Key = new FfcKeyPair(1, 2),
                Signature = new FfcSignature(3, 4),
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                DomainParams = new FfcDomainParameters(1, 2, 3)
            };
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, FfcVerificationResult>> GetDeferredMock(bool shouldPass)
        {
            var goodResult = Task.FromResult(new FfcVerificationResult());
            var badResult = Task.FromResult(new FfcVerificationResult("fail"));

            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, FfcVerificationResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
        }
    }
}
