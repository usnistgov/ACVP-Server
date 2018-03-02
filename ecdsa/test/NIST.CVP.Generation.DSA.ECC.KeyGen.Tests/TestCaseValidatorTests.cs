using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
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
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDsaFactoryMock(true).Object, GetCurveFactoryMock().Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadKey()
        {
            var subject = new TestCaseValidator(GetTestCase(), GetTestGroup(), GetDsaFactoryMock(false).Object, GetCurveFactoryMock().Object);
            var result = subject.Validate(GetResultTestCase());

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        private Mock<IDsaEcc> GetEccMock(bool shouldPass)
        {
            var eccMock = new Mock<IDsaEcc>();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(GetKey(shouldPass)));
            return eccMock;
        }

        private Mock<IDsaEccFactory> GetDsaFactoryMock(bool shouldPass)
        {
            var mock = new Mock<IDsaEccFactory>();
            mock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(GetEccMock(shouldPass).Object);
            return mock;
        }

        private EccKeyPair GetKey(bool correctKey)
        {
            return correctKey ? new EccKeyPair(new EccPoint(1, 2), 3) : new EccKeyPair(new EccPoint(4, 5), 6);
        }

        private Mock<IEccCurveFactory> GetCurveFactoryMock()
        {
            return new Mock<IEccCurveFactory>();
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
