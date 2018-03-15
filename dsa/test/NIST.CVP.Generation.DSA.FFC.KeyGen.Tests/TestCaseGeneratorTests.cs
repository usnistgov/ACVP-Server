using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnNonNullTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetDsaFactoryMock(GetDsaMock()).Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateXYIfIsSample()
        {
            var dsaMock = GetDsaMock();
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetDsaFactoryMock(dsaMock).Object);

            var result = subject.Generate(GetTestGroup(), true);

            dsaMock.Verify(v => v.GenerateKeyPair(It.IsAny<FfcDomainParameters>()), Times.Once, "Call KeyGen Generate once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One, testCase.Key.PrivateKeyX);
            Assert.AreEqual(BigInteger.One * 2, testCase.Key.PublicKeyY);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IDsaFfc> GetDsaMock()
        {
            var dsaMock = new Mock<IDsaFfc>();
            dsaMock
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(new FfcKeyPairGenerateResult(new FfcKeyPair(1, 2)));

            return dsaMock;
        }

        private Mock<IDsaFfcFactory> GetDsaFactoryMock(Mock<IDsaFfc> dsaMock)
        {
            var factoryMock = new Mock<IDsaFfcFactory>();
            factoryMock
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(dsaMock.Object);

            return factoryMock;
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                L = 2048,
                N = 224
            };
        }
    }
}
