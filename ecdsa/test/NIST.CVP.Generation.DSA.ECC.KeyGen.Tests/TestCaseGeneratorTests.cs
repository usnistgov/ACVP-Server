using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnNonNullTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateKeyPairIfIsSample()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success);
            var testCase = result.TestCase;
            Assert.AreEqual(BigInteger.One, testCase.KeyPair.PublicQ.X);
            Assert.AreEqual(BigInteger.One * 2, testCase.KeyPair.PublicQ.Y);
            Assert.AreEqual(BigInteger.One * 3, testCase.KeyPair.PrivateD);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IDsaEcc> GetEccMock()
        {
            var eccMock = new Mock<IDsaEcc>();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            return eccMock;
        }

        private Mock<IDsaEccFactory> GetEccFactoryMock()
        {
            var mock = new Mock<IDsaEccFactory>();
            mock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(GetEccMock().Object);
            return mock;
        }

        private Mock<IEccCurveFactory> GetCurveFactoryMock()
        {
            var mock = new Mock<IEccCurveFactory>();
            mock
                .Setup(s => s.GetCurve(It.IsAny<Curve>()))
                .Returns(new PrimeCurve(Curve.P192, 1, 2, new EccPoint(3, 4), 5));
            return mock;
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Curve = Curve.P192,
                SecretGenerationMode = SecretGenerationMode.TestingCandidates
            };
        }
    }
}
