using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
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
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetEccFactoryMock(true).Object, GetCurveFactoryMock().Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetEccFactoryMock(false).Object, GetCurveFactoryMock().Object);
            var result = subject.Validate(GetResultTestCase());

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
                Curve = Curve.P192,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
            };
        }

        private Mock<IDsaEccFactory> GetEccFactoryMock(bool success)
        {
            var mock = new Mock<IDsaEccFactory>();
            mock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(GetDsaMock(success).Object);
            return mock;
        }

        private Mock<IDsaEcc> GetDsaMock(bool success)
        {
            var eccMock = new Mock<IDsaEcc>();
            eccMock
                .Setup(s => s.Verify(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<EccSignature>(), It.IsAny<bool>()))
                .Returns(success ? new EccVerificationResult() : new EccVerificationResult("fail"));
            return eccMock;
        }

        private Mock<IEccCurveFactory> GetCurveFactoryMock()
        {
            return new Mock<IEccCurveFactory>();
        }
    }
}
