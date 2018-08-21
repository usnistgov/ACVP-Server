using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(true).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(false).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWhenDeferredCryptoFails()
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdKeyPairGenerateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new EdKeyPairGenerateResult("fail")));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), mock.Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        private EdKeyPair GetKey(bool correctKey)
        {
            return correctKey ? new EdKeyPair(14, 3) : new EdKeyPair(18, 6);
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdKeyPairGenerateResult>> GetDeferredResolver(bool shouldPass)
        {
            var goodResult = Task.FromResult(new EdKeyPairGenerateResult(GetKey(true)));
            var badResult = Task.FromResult(new EdKeyPairGenerateResult(GetKey(false)));

            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdKeyPairGenerateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
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
                KeyPair = GetKey(true)
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Curve = Curve.Ed25519,
                SecretGenerationMode = SecretGenerationMode.TestingCandidates
            };
        }
    }
}
