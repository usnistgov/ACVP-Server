using System.Numerics;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnNonNullTestCaseGenerateResponse()
        {
            var dsaMock = GetDsaMock();
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetDsaFactoryMock(dsaMock).Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateSignatureIfIsSample()
        {
            var dsaMock = GetDsaMock();
            dsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            dsaMock
                .Setup(s => s.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()))
                .Returns(new FfcSignatureResult(new FfcSignature(1, 2)));

            var subject = new TestCaseGenerator(GetRandomMock().Object, GetDsaFactoryMock(dsaMock).Object);

            var result = subject.Generate(GetTestGroup(), true);

            dsaMock.Verify(v => v.GenerateKeyPair(It.IsAny<FfcDomainParameters>()), Times.Once, "Call KeyGen Generate once");
            dsaMock.Verify(v => v.Sign(It.IsAny<FfcDomainParameters>(), It.IsAny<FfcKeyPair>(), It.IsAny<BitString>(), It.IsAny<bool>()), Times.Once, "Call Sign once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One, testCase.Key.PrivateKeyX);
            Assert.AreEqual(BigInteger.One * 2, testCase.Key.PublicKeyY);
            Assert.AreEqual(BigInteger.One, testCase.Signature.R);
            Assert.AreEqual(BigInteger.One * 2, testCase.Signature.S);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var randMock = new Mock<IRandom800_90>();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            return randMock;
        }

        private Mock<IDsaFfc> GetDsaMock()
        {
            return new Mock<IDsaFfc>();
        }

        private Mock<IDsaFfcFactory> GetDsaFactoryMock(Mock<IDsaFfc> dsaMock)
        {
            var mock = new Mock<IDsaFfcFactory>();
            mock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(dsaMock.Object);

            return mock;
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224,
                DomainParams = new FfcDomainParameters(1, 2, 3),
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256)
            };
        }
    }
}
