﻿using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.Ed;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithGoodSignature()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(true).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(false).Object);
            var result = await subject.ValidateAsync(GetResultTestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
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
                Signature = new EdSignature(5)
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Curve = Curve.Ed25519,
                PreHash = false,
                KeyPair = new EdKeyPair(new BitString("ec172b93ad5e563bf4932c70e1245034c35467ef2efd4d64ebf819683467e2bf").ToPositiveBigInteger())
            };
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdVerificationResult>> GetDeferredResolver(bool shouldPass)
        {
            var goodResult = Task.FromResult(new EdVerificationResult());
            var badResult = Task.FromResult(new EdVerificationResult("fail"));

            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdVerificationResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(shouldPass ? goodResult : badResult);

            return mock;
        }
    }
}
