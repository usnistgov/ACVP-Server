using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
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
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetEccMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateKeyPairIfIsSample()
        {
            var eccMock = GetEccMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            var subject = new TestCaseGenerator(GetRandomMock().Object, eccMock.Object);

            var result = subject.Generate(GetTestGroup(), true);

            eccMock.Verify(v => v.GenerateKeyPair(It.IsAny<EccDomainParameters>()), Times.Once, "Call KeyGen Generate once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
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
            return new Mock<IDsaEcc>();
        }

        private TestGroup GetTestGroup()
        {
            var curveFactory = new EccCurveFactory();
            var curve = curveFactory.GetCurve(Curve.P192);

            return new TestGroup
            {
                DomainParameters = new EccDomainParameters(curve, SecretGenerationMode.TestingCandidates)
            };
        }
    }
}
