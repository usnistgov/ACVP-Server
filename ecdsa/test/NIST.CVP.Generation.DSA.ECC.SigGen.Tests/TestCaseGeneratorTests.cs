using System.Numerics;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
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
            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            var subject = new TestCaseGenerator(randMock.Object, GetDsaMock().Object, GetShaFactoryMock().Object, GetCurveFactoryMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateSignatureIfIsSample()
        {
            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            var eccMock = GetDsaMock();
            eccMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2))));

            eccMock
                .Setup(s => s.Sign(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Returns(new EccSignatureResult(new EccSignature(3, 4)));

            var subject = new TestCaseGenerator(randMock.Object, eccMock.Object, GetShaFactoryMock().Object, GetCurveFactoryMock().Object);

            var result = subject.Generate(GetTestGroup(), true);

            eccMock.Verify(v => v.GenerateKeyPair(It.IsAny<EccDomainParameters>()), Times.Once, "Call KeyGen Generate once");
            eccMock.Verify(v => v.Sign(It.IsAny<EccDomainParameters>(), It.IsAny<EccKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()), Times.Once, "Call Sign once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One, testCase.KeyPair.PublicQ.X);
            Assert.AreEqual(BigInteger.One * 2, testCase.KeyPair.PublicQ.Y);
            Assert.AreEqual(BigInteger.One * 3, testCase.Signature.R);
            Assert.AreEqual(BigInteger.One * 4, testCase.Signature.S);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IDsaEcc> GetDsaMock()
        {
            return new Mock<IDsaEcc>();
        }

        private Mock<IEccCurveFactory> GetCurveFactoryMock()
        {
            return new Mock<IEccCurveFactory>();
        }

        private Mock<IShaFactory> GetShaFactoryMock()
        {
            return new Mock<IShaFactory>();
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
