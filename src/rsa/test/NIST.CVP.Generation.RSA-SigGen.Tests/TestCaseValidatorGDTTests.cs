using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Math;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Crypto.RSA2.Signatures;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGDTTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodSignature()
        {
            var mockSigner = GetResolverMock();
            mockSigner
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new VerifyResult());

            var subject = new TestCaseValidatorGDT(GetTestCase(), GetTestGroup(), mockSigner.Object);
            var result = subject.Validate(GetResultTestCase());

            mockSigner.Verify(v => v.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var mockSigner = GetResolverMock();
            mockSigner
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new VerifyResult("Fail"));

            var subject = new TestCaseValidatorGDT(GetTestCase(), GetTestGroup(), mockSigner.Object);
            var result = subject.Validate(GetResultTestCase());

            mockSigner.Verify(v => v.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, VerifyResult>> GetResolverMock()
        {
            return new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, VerifyResult>>();
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
