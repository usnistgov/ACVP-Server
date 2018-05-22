using System.Numerics;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorPQTests
    {
        [Test]
        public void GenerateShouldReturnEmptyTestCaseGenerateResponse()
        {
            var pqMock = new Mock<IPQGeneratorValidator>();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorPQ(GetRandomMock().Object, GetShaFactoryMock().Object, GetPQFactoryMock(pqMock).Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGeneratePQIfIsSample()
        {
            var pqMock = new Mock<IPQGeneratorValidator>();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorPQ(GetRandomMock().Object, GetShaFactoryMock().Object, GetPQFactoryMock(pqMock).Object);

            var result = subject.Generate(GetTestGroup(), true);

            pqMock.Verify(v => v.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once, "Call Generate once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;
            Assert.AreEqual(BigInteger.One, testCase.P);
            Assert.AreEqual(BigInteger.One * 2, testCase.Q);
            Assert.AreEqual(BigInteger.One * 3, testCase.Seed.GetFullSeed());
            Assert.AreEqual(4, testCase.Counter.Count);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IShaFactory> GetShaFactoryMock()
        {
            return new Mock<IShaFactory>();
        }

        private Mock<IPQGeneratorValidatorFactory> GetPQFactoryMock(Mock<IPQGeneratorValidator> pqMock)
        {
            var mock = new Mock<IPQGeneratorValidatorFactory>();
            mock
                .Setup(s => s.GetGeneratorValidator(It.IsAny<PrimeGenMode>(), It.IsAny<ISha>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(pqMock.Object);

            return mock;
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                PQGenMode = PrimeGenMode.Probable,
                L = 2048,
                N = 224,
                HashAlg = ShaAttributes.GetHashFunctionFromName("sha2-256")
            };
        }
    }
}
