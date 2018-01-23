using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodSignature()
        {
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.Verify(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<FfcSignature>(), It.IsAny<bool>()))
                .Returns(new FfcVerificationResult());

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), dsaMock.Object);
            var result = subject.Validate(GetResultTestCase());

            dsaMock.Verify(v => v.Verify(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<FfcSignature>(), It.IsAny<bool>()), Times.Once);

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.Verify(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<FfcSignature>(), It.IsAny<bool>()))
                .Returns(new FfcVerificationResult("Fail"));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), dsaMock.Object);
            var result = subject.Validate(GetResultTestCase());

            dsaMock.Verify(v => v.Verify(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<FfcSignature>(), It.IsAny<bool>()), Times.Once);

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
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
