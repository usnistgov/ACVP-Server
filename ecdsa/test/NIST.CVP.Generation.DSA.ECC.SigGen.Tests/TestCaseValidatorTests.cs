using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodSignature()
        {
            var eccMock = GetDsaMock();
            eccMock
                .Setup(s => s.Verify(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<EccSignature>()))
                .Returns(new EccVerificationResult());

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), eccMock.Object);
            var result = subject.Validate(GetResultTestCase());

            eccMock.Verify(v => v.Verify(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<EccSignature>()), Times.Once);

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var eccMock = GetDsaMock();
            eccMock
                .Setup(s => s.Verify(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<EccSignature>()))
                .Returns(new EccVerificationResult("Fail"));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), eccMock.Object);
            var result = subject.Validate(GetResultTestCase());

            eccMock.Verify(v => v.Verify(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<EccSignature>()), Times.Once);

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
                KeyPair = new EccKeyPair(new EccPoint(1, 2)),
                Signature = new EccSignature(3, 4),
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                DomainParameters = new EccDomainParameters(new PrimeCurve(Curve.P192, 0, 0, new EccPoint(0, 0), 0)),
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
            };
        }

        private Mock<IDsaEcc> GetDsaMock()
        {
            return new Mock<IDsaEcc>();
        }
    }
}
