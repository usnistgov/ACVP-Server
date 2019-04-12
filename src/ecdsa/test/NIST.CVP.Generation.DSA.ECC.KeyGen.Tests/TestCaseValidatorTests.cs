using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.ECDSA.v1_0.KeyGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.Tests
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
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EccKeyPairGenerateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new EccKeyPairGenerateResult("fail")));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), mock.Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
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
