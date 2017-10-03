using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodSignature()
        {
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.Verify(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<FfcSignature>()))
                .Returns(new FfcVerificationResult());

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), dsaMock.Object);
            var result = subject.Validate(GetResultTestCase());

            dsaMock.Verify(v => v.Verify(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<FfcSignature>()), Times.Once);

            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.Verify(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<FfcSignature>()))
                .Returns(new FfcVerificationResult("Fail"));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), dsaMock.Object);
            var result = subject.Validate(GetResultTestCase());

            dsaMock.Verify(v => v.Verify(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<FfcSignature>()), Times.Once);

            Assert.AreEqual("failed", result.Result);
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
                DomainParams = new FfcDomainParameters(1, 2, 3)
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
            };
        }

        private Mock<IDsaFfc> GetDsaMock()
        {
            return new Mock<IDsaFfc>();
        }
    }
}
