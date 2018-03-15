using System.Numerics;
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

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
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
        public void GenerateShouldGenerateSignatureIfIsSample()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetEccFactoryMock().Object, GetCurveFactoryMock().Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One * 3, testCase.Signature.R);
            Assert.AreEqual(BigInteger.One * 4, testCase.Signature.S);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var randMock = new Mock<IRandom800_90>();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));
            return randMock;
        }

        private Mock<IDsaEcc> GetDsaMock()
        {
            var eccMock = new Mock<IDsaEcc>();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2))));

            eccMock
                .Setup(s => s.Sign(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Returns(new EccSignatureResult(new EccSignature(3, 4)));

            return eccMock;
        }

        private Mock<IDsaEccFactory> GetEccFactoryMock()
        {
            var mock = new Mock<IDsaEccFactory>();
            mock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(GetDsaMock().Object);

            return mock;
        }

        private Mock<IEccCurveFactory> GetCurveFactoryMock()
        {
            return new Mock<IEccCurveFactory>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Curve = Curve.P192,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256)
            };
        }
    }
}
