using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.RSA.v1_0.SigGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodSignature()
        {
            var mockSigner = GetResolverMock();
            mockSigner
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new VerifyResult()));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), mockSigner.Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            mockSigner.Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var mockSigner = GetResolverMock();
            mockSigner
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new VerifyResult("Fail")));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), mockSigner.Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            mockSigner.Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, VerifyResult>> GetResolverMock()
        {
            return new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, VerifyResult>>();
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                Message = new BitString("ABCD"),
                TestCaseId = 1
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Mode = SignatureSchemes.Ansx931,
                Modulo = 2048,
                Key = new KeyPair()
            };
        }

        private TestCase GetResultTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                Signature = new BitString("ABCD")
            };
        }
    }
}
