using System.Numerics;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorGTests
    {
        [Test]
        public void GenerateShouldReturnNonNullTestCaseGenerateResponse()
        {
            var gMock = new Mock<IGGeneratorValidator>();
            gMock
                .Setup(s => s.Generate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<BitString>()))
                .Returns(new GGenerateResult(1));

            var subject = new TestCaseGeneratorG(GetRandomMock().Object, GetShaFactoryMock().Object, GetPQFactoryMock().Object, GetGFactoryMock(gMock).Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateGIfIsSample()
        {
            var gMock = new Mock<IGGeneratorValidator>();
            gMock
                .Setup(s => s.Generate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<BitString>()))
                .Returns(new GGenerateResult(1));

            var subject = new TestCaseGeneratorG(GetRandomMock().Object, GetShaFactoryMock().Object, GetPQFactoryMock().Object, GetGFactoryMock(gMock).Object);
            var result = subject.Generate(GetTestGroup(), true);

            gMock.Verify(v => v.Generate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<BitString>()), Times.Once, "Call Generate once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One, testCase.P);
            Assert.AreEqual(BigInteger.One * 2, testCase.Q);
            Assert.AreEqual(BigInteger.One * 3, testCase.Seed.GetFullSeed());
            Assert.AreEqual(4, testCase.Counter.Count);
            Assert.AreEqual(BigInteger.One, testCase.G);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var rand = new Mock<IRandom800_90>();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("AB"));

            return rand;
        }

        private Mock<IGGeneratorValidatorFactory> GetGFactoryMock(Mock<IGGeneratorValidator> gMock)
        {
            var mock = new Mock<IGGeneratorValidatorFactory>();
            mock
                .Setup(s => s.GetGeneratorValidator(It.IsAny<GeneratorGenMode>(), It.IsAny<ISha>()))
                .Returns(gMock.Object);

            return mock;
        }

        private Mock<IShaFactory> GetShaFactoryMock()
        {
            return new Mock<IShaFactory>();
        }

        private Mock<IPQGeneratorValidatorFactory> GetPQFactoryMock()
        {
            var pqMock = new Mock<IPQGeneratorValidator>();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var mock = new Mock<IPQGeneratorValidatorFactory>();
            mock
                .Setup(s => s.GetGeneratorValidator(It.IsAny<PrimeGenMode>(), It.IsAny<ISha>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(pqMock.Object);

            return mock;
        }

        private TestGroup GetTestGroup()
        {
            var attributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm("sha2-256");

            return new TestGroup
            {
                GGenMode = GeneratorGenMode.Canonical,
                L = 2048,
                N = 224,
                HashAlg = new HashFunction(attributes.shaMode, attributes.shaDigestSize)
            };
        }
    }
}
