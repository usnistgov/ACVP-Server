using System.Linq;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.PQGVer.TestCaseExpectations;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorPQTests
    {
        [Test]
        public void GenerateShouldReturnNonNullTestCaseGenerateResponse()
        {
            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorPQ(GetRandomMock().Object, GetShaFactoryMock().Object, GetPQFactoryMock(pqMock).Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGeneratePQ()
        {
            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorPQ(GetRandomMock().Object, GetShaFactoryMock().Object, GetPQFactoryMock(pqMock).Object);
            var result = subject.Generate(GetTestGroup(), true);

            pqMock.Verify(v => v.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once, "Call Generate once");

            Assert.IsTrue(result.Success);
            var testCase = result.TestCase;
            Assert.AreEqual(4, testCase.Counter.Count);
        }

        [Test]
        public void GenerateShouldGenerateOneOfEachFailureReason()
        {
            var group = GetTestGroup();

            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorPQ(GetRandomMock().Object, GetShaFactoryMock().Object, GetPQFactoryMock(pqMock).Object);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(group, false);

                Assert.IsTrue(result.Success);
                group.Tests.Add(result.TestCase);
            }

            var failCases = 0;
            var passCases = 0;
            foreach (var testCase in group.Tests.Select(s => s))
            {
                if (testCase.TestPassed != null && testCase.TestPassed.Value)
                {
                    passCases++;
                }
                else
                {
                    failCases++;
                }
            }

            Assert.AreEqual(3, failCases);
            Assert.AreEqual(2, passCases);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var randMock = new Mock<IRandom800_90>();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));
            
            return randMock;
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

        private Mock<IPQGeneratorValidator> GetPQMock()
        {
            return new Mock<IPQGeneratorValidator>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                PQGenMode = PrimeGenMode.Probable,
                PQTestCaseExpectationProvider = new PQTestCaseExpectationProvider(),
                L = 2048,
                N = 224,
                HashAlg = ShaAttributes.GetHashFunctionFromName("sha2-256")
            };
        }
    }
}
