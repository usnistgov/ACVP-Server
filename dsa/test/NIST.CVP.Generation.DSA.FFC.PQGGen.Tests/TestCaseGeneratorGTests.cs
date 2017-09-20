using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorGTests
    {
        [Test]
        public void GenerateShouldReturnEmptyTestCaseGenerateResponse()
        {
            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("AB"));

            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var subject = new TestCaseGeneratorG(rand.Object, GetGMock().Object, pqMock.Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateGIfIsSample()
        {
            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("AB"));

            var pqMock = GetPQMock();
            pqMock
                .Setup(s => s.Generate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new PQGenerateResult(1, 2, new DomainSeed(3), new Counter(4)));

            var gMock = GetGMock();
            gMock
                .Setup(s => s.Generate(It.IsAny<BigInteger>(), It.IsAny<BigInteger>(), It.IsAny<DomainSeed>(), It.IsAny<BitString>()))
                .Returns(new GGenerateResult(1));

            var subject = new TestCaseGeneratorG(rand.Object, gMock.Object, pqMock.Object);

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
            return new Mock<IRandom800_90>();
        }

        private Mock<IGGeneratorValidator> GetGMock()
        {
            return new Mock<IGGeneratorValidator>();
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
                GGenMode = GeneratorGenMode.Canonical,
                L = 2048,
                N = 224,
                HashAlg = new HashFunction(attributes.shaMode, attributes.shaDigestSize)
            };
        }
    }
}
