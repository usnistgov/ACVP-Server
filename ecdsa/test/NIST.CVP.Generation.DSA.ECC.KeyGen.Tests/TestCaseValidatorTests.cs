using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core.Enums;
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
            var eccMock = GetEccMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), eccMock.Object);
            var result = subject.Validate(GetResultTestCase());

            eccMock.Verify(v => v.GenerateKeyPair(It.IsAny<EccDomainParameters>()), Times.Once);

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var eccMock = GetEccMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(4, 5), 6)));  // Not the supplied key

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), eccMock.Object);
            var result = subject.Validate(GetResultTestCase());

            eccMock.Verify(v => v.GenerateKeyPair(It.IsAny<EccDomainParameters>()), Times.Once);

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        private Mock<IDsaEcc> GetEccMock()
        {
            return new Mock<IDsaEcc>();
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
                KeyPair = new EccKeyPair(new EccPoint(1, 2), 3)
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                DomainParameters = new EccDomainParameters(new PrimeCurve(Curve.P224, 0, 0, new EccPoint(0, 0), 0), SecretGenerationMode.TestingCandidates)
            };
        }
    }
}
