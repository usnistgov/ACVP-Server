using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodKey()
        {
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.ValidateKeyPair(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>()))
                .Returns(new FfcKeyPairValidateResult());

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), dsaMock.Object);
            var result = subject.Validate(GetResultTestCase());

            dsaMock.Verify(v => v.ValidateKeyPair(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>()), Times.Once);

            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.ValidateKeyPair(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>()))
                .Returns(new FfcKeyPairValidateResult("Fail"));

            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), dsaMock.Object);
            var result = subject.Validate(GetResultTestCase());

            dsaMock.Verify(v => v.ValidateKeyPair(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>()), Times.Once);

            Assert.AreEqual("failed", result.Result);
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
                Key = new FfcKeyPair(1, 2)
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224,
                DomainParams = new FfcDomainParameters(1, 2, 3)
            };
        }

        private Mock<IDsaFfc> GetDsaMock()
        {
            return new Mock<IDsaFfc>();
        }
    }
}
