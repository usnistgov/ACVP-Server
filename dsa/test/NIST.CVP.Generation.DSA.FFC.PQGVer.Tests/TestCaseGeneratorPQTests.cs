using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
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
            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorPQ(randMock.Object, pqMock.Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGeneratePQ()
        {
            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorPQ(randMock.Object, pqMock.Object);

            var result = subject.Generate(GetTestGroup(), true);

            pqMock.Verify(v => v.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once, "Call Generate once");

            Assert.IsTrue(result.Success);
            var testCase = (TestCase)result.TestCase;

            // These values could change
            // Assert.AreEqual(BigInteger.One, testCase.P);
            // Assert.AreEqual(BigInteger.One * 2, testCase.Q);
            // Assert.AreEqual(BigInteger.One * 3, testCase.Seed.GetFullSeed());

            Assert.AreEqual(4, testCase.Counter.Count);
        }

        [Test]
        public void GenerateShouldGenerateOneOfEachFailureReason()
        {
            var group = GetTestGroup();

            var randMock = GetRandomMock();
            randMock
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCD"));

            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorPQ(randMock.Object, pqMock.Object);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(group, false);

                Assert.IsTrue(result.Success);
                group.Tests.Add(result.TestCase);
            }

            Assert.AreEqual(0, group.PQCovered.Count);

            var failCases = 0;
            var passCases = 0;
            foreach (var testCase in group.Tests.Select(s => (TestCase)s))
            {
                if (testCase.FailureTest)
                {
                    failCases++;
                }
                else
                {
                    passCases++;
                }
            }

            Assert.AreEqual(3, failCases);
            Assert.AreEqual(2, passCases);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IPQGeneratorValidator> GetPQMock()
        {
            return new Mock<IPQGeneratorValidator>();
        }

        private TestGroup GetTestGroup()
        {
            var attributes = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm("sha2-256");

            return new TestGroup
            {
                PQGenMode = PrimeGenMode.Probable,
                L = 2048,
                N = 224,
                HashAlg = new HashFunction(attributes.shaMode, attributes.shaDigestSize)
            };
        }
    }
}
