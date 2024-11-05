using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.KeyGen
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(true).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(false).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWhenDeferredCryptoFails()
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EccKeyPairGenerateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new EccKeyPairGenerateResult("fail")));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), mock.Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
        }

        private EccKeyPair GetKey(bool correctKey)
        {
            return correctKey ? new EccKeyPair(new EccPoint(1, 2), 3) : new EccKeyPair(new EccPoint(4, 5), 6);
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EccKeyPairGenerateResult>> GetDeferredResolver(bool shouldPass)
        {
            var goodResult = Task.FromResult(new EccKeyPairGenerateResult(GetKey(true)));
            var badResult = Task.FromResult(new EccKeyPairGenerateResult(GetKey(false)));

            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EccKeyPairGenerateResult>>();
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
                Curve = Curve.P224,
                SecretGenerationMode = SecretGenerationMode.TestingCandidates
            };
        }
    }
}
