using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.KeyGen
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
            return correctKey ? new EdKeyPair(new Math.BitString("9d61b19deffd5a60ba844af492ec2cc44449c5697b326919703bac031cae7f60"), new Math.BitString("d75a980182b10ab7d54bfed3c964073a0ee172f3daa62325af021a68f707511a")) : new EdKeyPair(new Math.BitString("FACE"), new Math.BitString("BEEF"));
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
