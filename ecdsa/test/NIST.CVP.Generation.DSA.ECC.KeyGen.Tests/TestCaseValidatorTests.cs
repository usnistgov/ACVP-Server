using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(true).Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDeferredResolver(false).Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWhenDeferredCryptoFails()
        {
            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, EccKeyPairGenerateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new EccKeyPairGenerateResult("fail"));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), mock.Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        private EccKeyPair GetKey(bool correctKey)
        {
            return correctKey ? new EccKeyPair(new EccPoint(1, 2), 3) : new EccKeyPair(new EccPoint(4, 5), 6);
        }

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, EccKeyPairGenerateResult>> GetDeferredResolver(bool shouldPass)
        {
            var goodResult = new EccKeyPairGenerateResult(GetKey(true));
            var badResult = new EccKeyPairGenerateResult(GetKey(false));

            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, EccKeyPairGenerateResult>>();
            mock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
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
