using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SigGen
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

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var mockSigner = GetResolverMock();

            var suppliedTestCase = GetResultTestCase();
            suppliedTestCase.ParentGroup.Key.PubKey.E = 3;

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), mockSigner.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            mockSigner.Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Never);
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
                Signature = new BitString("ABCD"),
                ParentGroup = new TestGroup
                {
                    Key = new KeyPair { PubKey = new PublicKey { E = 65537, N = BitString.Ones(2048).ToPositiveBigInteger() } }
                }
            };
        }
    }
}
